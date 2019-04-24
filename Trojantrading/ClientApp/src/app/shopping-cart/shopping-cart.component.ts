import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { Product } from '../models/Product';
import { ShareService } from '../services/share.service';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { ShoppingCart } from '../models/shoppingCart';
import { AdminService } from '../services/admin.service';
import { User } from '../models/user';
import { Router } from '@angular/router';
import { ShoppingItem } from '../models/shoppingItem';
import { ApiResponse } from '../models/ApiResponse';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit {

  dataSource: ShoppingItem[];
  displayedColumns: string[] = [];
  shoppingCart: ShoppingCart;
  priceExclGst: number;
  oringinalPriceIncGst: number;
  oringinalPriceExclGst: number;
  gst: number;
  priceIncGst: number;
  discount: number;
  role: string = this.shareService.readCookie("role");

  constructor(
    private nav: NavbarService,
    private shareService: ShareService,
    private shoppingCartService: ShoppingCartService,
    private adminService: AdminService,
    private router: Router
  ) {
    if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "agent") {
      this.displayedColumns = ['name', 'category', 'originalPrice', 'agentPrice', 'qty', 'subTotal', 'remove'];
    }
    else if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "reseller") {
      this.displayedColumns = ['name', 'category', 'originalPrice', 'resellerPrice', 'qty', 'subTotal', 'remove'];
    }
  }

  ngOnInit() {
    this.nav.hideTab();
    this.nav.show();

    this.shoppingCartService.currentShoppingItemLength.subscribe((length: number) => {
      this.adminService.GetUserByAccount(this.shareService.readCookie("userName")).subscribe((user: User) => {
        this.shoppingCartService.GetShoppingCart(user.id).subscribe((res: ShoppingCart) => {
          this.priceExclGst = 0;
          this.gst = 0;
          this.priceIncGst = 0;
          this.oringinalPriceExclGst = 0;
          this.oringinalPriceIncGst = 0;
          this.discount = 0;
          this.shoppingCart = res;
          this.dataSource = this.shoppingCart.shoppingItems;
          if (res.shoppingItems.length) {
            this.dataSource.forEach(si => {
              this.oringinalPriceExclGst += si.amount * si.product.originalPrice;
              if (this.role == "agent") {
                si.subTotal = si.amount * si.product.agentPrice;
                this.priceExclGst += si.amount * si.product.agentPrice;
              } else if (this.role == "reseller") {
                si.subTotal = si.amount * si.product.agentPrice;
                this.priceExclGst += si.amount * si.product.resellerPrice;
              }
            });
            this.gst = this.priceExclGst * 0.1;
            this.oringinalPriceIncGst = this.oringinalPriceExclGst + this.oringinalPriceExclGst * 0.1;
            this.priceIncGst = this.gst + this.priceExclGst;
            this.discount = this.oringinalPriceIncGst - this.priceIncGst;
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

    this.adminService.GetUserByAccount(this.shareService.readCookie("userName")).subscribe((user: User) => {
      this.shoppingCartService.GetShoppingCart(user.id).subscribe((res: ShoppingCart) => {
        this.priceExclGst = 0;
        this.gst = 0;
        this.priceIncGst = 0;
        this.oringinalPriceExclGst = 0;
        this.oringinalPriceIncGst = 0;
        this.discount = 0;
        this.shoppingCart = res;
        this.dataSource = this.shoppingCart.shoppingItems;
        if (res.shoppingItems.length) {
          this.dataSource.forEach(si => {
            this.oringinalPriceExclGst += si.amount * si.product.originalPrice;
            if (this.role == "agent") {
              si.subTotal = si.amount * si.product.agentPrice;
              this.priceExclGst += si.amount * si.product.agentPrice;
            } else if (this.role == "reseller") {
              si.subTotal = si.amount * si.product.agentPrice;
              this.priceExclGst += si.amount * si.product.resellerPrice;
            }
          });
          this.gst = this.priceExclGst * 0.1;
          this.oringinalPriceIncGst = this.oringinalPriceExclGst + this.oringinalPriceExclGst * 0.1;
          this.priceIncGst = this.gst + this.priceExclGst;
          this.discount = this.oringinalPriceIncGst - this.priceIncGst;
        }
      },
        (error: any) => {
          console.info(error);
        });
    },
      (error: any) => {
        console.info(error);
      });

  }

  continueShopping(): void {
    this.router.navigate(['/home']);
  }

  deleteShoppingItem(shoppingItem: ShoppingItem): void{
    this.shoppingCartService.DeleteShoppingItem(shoppingItem.id).subscribe((res: ApiResponse) => {
      if(res && res.status == "success"){
        this.adminService.GetUserByAccount(this.shareService.readCookie("userName")).subscribe((user: User) => {
          this.shoppingCartService.GetShoppingCart(user.id).subscribe((res: ShoppingCart) => {
            this.priceExclGst = 0;
            this.gst = 0;
            this.priceIncGst = 0;
            this.discount = 0;
            this.shoppingCart = res;
            this.dataSource = this.shoppingCart.shoppingItems;
            this.shoppingCartService.MonitorShoppingItemLength(this.dataSource.length);
            if (res && res.shoppingItems.length) {
              this.dataSource.forEach(si => {
                this.oringinalPriceExclGst = si.amount * si.product.originalPrice;
                if (this.role == "agent") {
                  si.subTotal = si.amount * si.product.agentPrice;
                  this.priceExclGst += si.amount * si.product.agentPrice;
                } else if (this.role == "reseller") {
                  si.subTotal = si.amount * si.product.agentPrice;
                  this.priceExclGst += si.amount * si.product.resellerPrice;
                }
              });
              this.gst = this.priceExclGst * 0.1;
              this.oringinalPriceIncGst = this.oringinalPriceExclGst + this.oringinalPriceExclGst * 0.1;
              this.priceIncGst = this.gst + this.priceExclGst;
              this.discount = this.oringinalPriceIncGst - this.priceIncGst;
            }
          },
            (error: any) => {
              console.info(error);
            });
        },
          (error: any) => {
            console.info(error);
          });
      }else{
        this.shareService.showError("shoppingItem"+shoppingItem.id, res.message, "right");
      }
    },
      (error: any) => {
        console.info(error);
      });
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
