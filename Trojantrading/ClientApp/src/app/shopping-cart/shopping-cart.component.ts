import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { Product } from '../models/Product';
import { ShareService } from '../services/share.service';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { ShoppingCart } from '../models/shoppingCart';
import { AdminService } from '../services/admin.service';
import { User } from '../models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit {

  dataSource: Product[];
  totalPrice: number;
  displayedColumns: string[] = ['name', 'category', 'originalPrice', 'qty', 'subTotal', 'remove'];
  shoppingCart: ShoppingCart;

  constructor(
    private nav: NavbarService,
    private shareService: ShareService,
    private shoppingCartService: ShoppingCartService,
    private adminService: AdminService,
    private router: Router
  ) { }

  ngOnInit() {
    this.nav.hideTab();
    this.totalPrice = 0;
    this.adminService.GetUserByAccount(this.shareService.readCookie("role")).subscribe((user: User) => {
      this.shoppingCartService.GetShoppingCart(user.id).subscribe((res: ShoppingCart) => {
        this.shoppingCart = res;
        console.info(this.shoppingCart);
      },
        (error: any) => {
          console.info(error);
        });
    },
      (error: any) => {
        console.info(error);
      });

  }

  continueShopping(): void{
    this.router.navigate(['/home']);
  }

  _keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    let inputChar = String.fromCharCode(event.charCode);

    if (!pattern.test(inputChar)) {
      // invalid character, prevent input
      event.preventDefault();
    }
  }
}
