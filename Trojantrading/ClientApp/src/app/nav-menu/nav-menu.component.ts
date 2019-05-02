import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ShareService } from '../services/share.service';
import { NgbDropdownConfig, NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { AdminService } from '../services/admin.service';
import { User } from '../models/user';
import { ShoppingCart } from '../models/shoppingCart';
import { ShoppingItem } from '../models/shoppingItem';
import { ApiResponse } from '../models/ApiResponse';
import * as _ from 'lodash';
import { Category } from '../models/Product';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  providers: [NgbDropdownConfig]
})
export class NavMenuComponent implements OnInit {

  shoppingItems: ShoppingItem[];
  role: string;
  categoryList: Category[];

  constructor(
    public nav: NavbarService,
    private router: Router,
    private activatedRouter: ActivatedRoute,
    private shareService: ShareService,
    private shoppingCartService: ShoppingCartService,
    private adminService: AdminService,
    private productService: ProductService
  ) { 
  }

  ngOnInit() {
    this.role = this.shareService.readCookie("role");
    this.categoryList = this.productService.categoryList;

    this.shoppingCartService.currentShoppingItemLength.subscribe((length: number) => {
      this.shoppingCartService.AddShoppingCart(_.toNumber(this.shareService.readCookie("userId"))).subscribe((sc: ApiResponse) => {
        this.shoppingCartService.GetShoppingCart(_.toNumber(this.shareService.readCookie("userId"))).subscribe((res: ShoppingCart) => {
          if(res && res.shoppingItems.length){
            this.shoppingItems = res.shoppingItems;
          }
        },
          (error: any) => {
            console.info(error);
          });
      },
        (error: any) => {
          console.info(error);
        });
    },
      (error: any) => {
        console.info(error);
      });
  }

  proceedToCheckout(): void {
    this.router.navigate(["/cart"]);
  }

  deleteShoppingItem(shoppingItem: ShoppingItem): void {
    this.shoppingCartService.DeleteShoppingItem(shoppingItem.id).subscribe((res: ApiResponse) => {
      if (res && res.status == "success") {
        this.adminService.GetUserByAccount(_.toNumber(this.shareService.readCookie("userId"))).subscribe((user: User) => {
          this.shoppingCartService.GetShoppingCart(user.id).subscribe((res: ShoppingCart) => {
            this.shoppingItems = res.shoppingItems;
            this.shoppingCartService.MonitorShoppingItemLength(this.shoppingItems.length);
          },
            (error: any) => {
              console.info(error);
            });
        },
          (error: any) => {
            console.info(error);
          });
      } else {
        this.shareService.showError("shoppingItem" + shoppingItem.id, res.message, "right");
      }
    },
      (error: any) => {
        console.info(error);
      });
  }

  continueShopping(): void {
    this.router.navigate(['/home']);
  }


  logOut(): void {
    this.shareService.savecookies("userToken", "", 1);
    this.router.navigate(['login'], { relativeTo: this.activatedRouter.parent })
  }

}
