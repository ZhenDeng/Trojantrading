import { FileService } from './../services/file.service';
import { MatTableDataSource } from '@angular/material';
import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';
import { OrderService } from '../services/order.service';
import { Order } from '../models/order';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
  providers: [DatePipe]
})
export class OrdersComponent implements OnInit {

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

    this.convertDateFormat();

    this.orderService.getOrdersByUserID(this.userId, this.strDateFrom, this.strDateTo).subscribe((value: Order[]) => {
      console.info(value);
      this.orders = value;
      this.dataSource = new MatTableDataSource(this.orders);
    },
    (error: any) => {
      console.info(error);
    });

  }

  convertDateFormat(): void {
    this.strDateFrom = this.dateFrom.day + '/' + this.dateFrom.month + '/' + this.dateFrom.year;
    this.strDateTo = this.dateTo.day + '/' + this.dateTo.month + '/' + this.dateTo.year;
  }

  downloadExcel() {
    this.convertDateFormat();

    let exportOrdersArray = this.orders.map(x => ({
      InvoiceNumber: x.invoiceNo,
      Customer: x.user.bussinessName,
      CreatedDate: this.datePipe.transform(x.createdDate, 'yyyy-MM-dd'),
      Amount: x.totalPrice,
      Status: x.orderStatus
    }));


    this.fileService.exportAsExcelFile(exportOrdersArray, 'TrojanTrading_Orders_' + this.strDateFrom + '_' + this.strDateTo);

  }

  onLoading(currentLoadingStatus: boolean) {
    this.loadContent = !currentLoadingStatus;
  }

  applyFilter(value: string) {
    value = value.trim();
    value = value.toLowerCase();
    this.dataSource.filter = value;
  }

}
