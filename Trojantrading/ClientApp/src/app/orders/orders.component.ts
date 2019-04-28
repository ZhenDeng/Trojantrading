import { ProductService } from './../services/product.service';
import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { AdminService } from '../services/admin.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {

  constructor(
    private nav: NavbarService,
    private shareService: ShareService,
    private productService: ProductService,
    private shoppingCartService: ShoppingCartService,
    private adminService: AdminService,
    private router: Router
  ) { }

  ngOnInit() {
    this.nav.hideTab();
    this.nav.show();
  }

  //getAllOrders()

}
