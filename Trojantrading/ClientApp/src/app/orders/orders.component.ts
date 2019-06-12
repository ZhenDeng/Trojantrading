import { Product } from './../models/Product';
import { FileService } from './../services/file.service';
import { MatTableDataSource } from '@angular/material';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';
import { OrderService } from '../services/order.service';
import { Order } from '../models/order';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date';
import { DatePipe } from '@angular/common';
import { Subject } from 'rxjs';
import 'rxjs/add/operator/takeUntil';

declare let alasql;

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
  providers: [DatePipe]
})
export class OrdersComponent implements OnInit, OnDestroy {

  title = 'Order History';
  url = '';
  userId: string = '';
  role: string = '';
  dateFrom: NgbDate;
  dateTo: NgbDate;
  strDateFrom: string = '';
  strDateTo: string = '';
  orders: Order[] = [];
  filteredOrder: Order[] = [];
  dataSource = new MatTableDataSource();
  loadContent: boolean = false;
  displayedColumns: string[] = ['invoiceNo', 'customer', 'createdDate', 'totalPrice', 'payment', 'orderStatus'];
  
   //unsubscribe
   ngUnsubscribe: Subject<void> = new Subject<void>();

  constructor(
    private nav: NavbarService,
    private shareService: ShareService,
    private orderService: OrderService,
    private fileService: FileService,
    private calendar: NgbCalendar,
    private datePipe: DatePipe
  ) { }

  ngOnInit() {
    this.dateFrom = this.calendar.getNext(this.calendar.getToday(), 'd', -30);
    this.dateTo = this.calendar.getToday();

    this.nav.show();

    this.role = this.shareService.readCookie("role");

    //admin user parse empty userid to get all other users' order records
    this.userId = this.role === "admin" ? '' : this.shareService.readCookie("userId");

    this.getOrders();

  }

  getOrders() {
    
    this.convertDateFormat();

    this.orderService.getOrdersByUserID(this.userId, this.strDateFrom, this.strDateTo).takeUntil(this.ngUnsubscribe)
    .subscribe((value: Order[]) => {
      console.info(value);
      this.orders = value;
      this.dataSource = new MatTableDataSource(this.orders);
    },
    (error: any) => {
      console.info(error);
    });
  }

  convertDateFormat(): void {
    this.strDateFrom = this.dateFrom.month + '/' + this.dateFrom.day + '/' + this.dateFrom.year;
    this.strDateTo = this.dateTo.month + '/' + this.dateTo.day + '/' + this.dateTo.year;
  }

  downloadExcel() {
    this.convertDateFormat();
    if (this.orders.length > 0) {
      let exportArray = [];
      let optArray = [];
      this.orders.forEach(order => {
        let itemArray = [];
         order.shoppingCart.shoppingItems.forEach(item => {
          itemArray.push({
             "Item Code": item.product.itemCode,
             "Product Name": item.product.name,
             "Packaging": item.product.packaging,
             "Original Price": `$${item.product.originalPrice}`,
             "Buy Price": '$' + this.getBuyPrice(item.product),
             "Order Date": this.datePipe.transform(order.createdDate,'dd/MM/yyyy'),
             "Order Qty": item.amount,
             "Line Amount": '$' + (item.amount * this.getBuyPrice(item.product)).toFixed(2)
           })
         });
         exportArray.push(itemArray);
         optArray.push({"sheetid": 'Order_' +order.id, "header": true });
      });

      const fileName = 'TrojanTrading_Orders_' + this.strDateFrom + '_' + this.strDateTo;
      let res = alasql('SELECT INTO XLSX("' + fileName +'.xlsx",?) FROM ?',
                       [optArray,exportArray]);

    } else {
      this.shareService.openSnackBar('No available data for export', 'error');
    }

    // this.fileService.exportAsExcelFile(exportOrdersArray, 'TrojanTrading_Orders_' + this.strDateFrom + '_' + this.strDateTo);

  }

  onLoading(currentLoadingStatus: boolean) {
    this.loadContent = !currentLoadingStatus;
  }

  getBuyPrice(product: Product):number {
    if (this.role === 'agent') {
      return product.agentPrice;
    } else if (this.role === 'wholesaler') {
      return product.wholesalerPrice;
    } else {
      return product.originalPrice;
    }
  }

  applyFilter(value: string) {
    value = value.trim();
    value = value.toLowerCase();
    this.dataSource.filter = value;
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

}
