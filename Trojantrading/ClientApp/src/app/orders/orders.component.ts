import { MatTableDataSource } from '@angular/material';
import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';
import { Router } from '@angular/router';
import { OrderService } from '../services/order.service';
import { Order } from '../models/order';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date';


@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {

  title = 'Order History';
  userId: string = '';
  role: string = '';
  dateFrom: NgbDate;
  dateTo: NgbDate;
  strDateFrom: string = '';
  strDateTo: string = '';
  orders: Order[] = [];
  filteredOrder: Order[] = [];
  dataSource = new MatTableDataSource();
  displayedColumns: string[] = ['id', 'customer', 'createdDate', 'totalPrice', 'orderStatus', 'button'];

  constructor(
    private nav: NavbarService,
    private shareService: ShareService,
    private orderService: OrderService,
    private calendar: NgbCalendar,
    private router: Router
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

  convertDateFormat(): void {
    this.strDateFrom = this.dateFrom.day + '/' + this.dateFrom.month + '/' + this.dateFrom.year;
    this.strDateTo = this.dateTo.day + '/' + this.dateTo.month + '/' + this.dateTo.year;
  }


  getOrders() {
    this.convertDateFormat();

    this.orderService.getOrdersByUserID(this.userId, this.strDateFrom, this.strDateTo).subscribe((value: Order[]) => {
       this.orders = value;
       this.orders.forEach(x => x.customer = x.user.bussinessName);
       this.dataSource = new MatTableDataSource(this.orders);
    },
    (error: any) => {
      console.info(error);
    });
  }

  applyFilter(value: string) {
    value = value.trim();
    value = value.toLowerCase();
    this.dataSource.filter = value;
  }

}
