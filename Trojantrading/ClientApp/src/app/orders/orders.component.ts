import { MatTableDataSource } from '@angular/material';
import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { AdminService } from '../services/admin.service';
import { Router } from '@angular/router';
import { OrderService } from '../services/order.service';
import { Order } from '../models/order';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {

  title = 'Order History';
  userId: string = '';
  role: string = '';
  dateFrom: string = '';
  dateTo: string = '';
  orders: Order[] = [];
  filteredOrder: Order[] = [];
  dataSource = new MatTableDataSource();
  displayedColumns: string[] = ['Id', 'bussinessName', 'createdDate', 'totalPrice', 'orderStatus', 'button'];

  constructor(
    private nav: NavbarService,
    private shareService: ShareService,
    private orderService: OrderService,
    private shoppingCartService: ShoppingCartService,
    private adminService: AdminService,
    private router: Router
  ) { }

  ngOnInit() {
    this.nav.show();

    this.role = this.shareService.readCookie("role");

    //admin user parse empty userid to get all other users' order records
    this.userId = this.role === "admin" ? '' : this.shareService.readCookie("userId");

    this.getOrders();
  }

  getOrders() {

    this.orderService.getOrdersByUserID(this.userId, this.dateFrom, this.dateTo).subscribe((value: Order[]) => {
       this.orders = value;
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
