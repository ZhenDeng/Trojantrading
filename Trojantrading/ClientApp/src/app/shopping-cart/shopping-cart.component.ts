import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { ShoppingCart } from '../models/shoppingCart';
import { AdminService } from '../services/admin.service';
import { User } from '../models/user';
import { Router } from '@angular/router';
import { ShoppingItem } from '../models/shoppingItem';
import { ApiResponse } from '../models/ApiResponse';
import { OrderService } from '../services/order.service';
import * as _ from 'lodash';

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
  successCheckout: boolean = false;
  selectedPayment: string;

  constructor(
    private nav: NavbarService,
    private shareService: ShareService,
    private shoppingCartService: ShoppingCartService,
    private adminService: AdminService,
    private router: Router,
    private orderService: OrderService
  ) {
    if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "agent") {
      this.displayedColumns = ['name', 'category', 'originalPrice', 'agentPrice', 'qty', 'subTotal', 'remove'];
    }
    else if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "wholesaler") {
      this.displayedColumns = ['name', 'category', 'originalPrice', 'wholesalerPrice', 'qty', 'subTotal', 'remove'];
    }
  }

  ngOnInit() {

    this.nav.show();
    this.selectedPayment = "onaccount";
    this.successCheckout = false;
    this.shoppingCartService.currentShoppingItemLength.subscribe((length: number) => {
      this.adminService.GetUserByAccount(_.toNumber(this.shareService.readCookie("userId"))).subscribe((user: User) => {
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
                this.priceExclGst += si.subTotal;
              } else if (this.role == "wholesaler") {
                si.subTotal = si.amount * si.product.wholesalerPrice;
                this.priceExclGst += si.subTotal;
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
  }

  continueShopping(): void {
    this.successCheckout = false;
    this.router.navigate(['/home']);
  }

  checkoutShoppingItems(): void {
    this.shoppingCart.paymentMethod = this.selectedPayment;
    this.orderService.AddOrder(this.shoppingCart).subscribe((res: ApiResponse) => {
      if (res && res.status == "success") {
        this.adminService.GetUserByAccount(_.toNumber(this.shareService.readCookie("userId"))).subscribe((user: User) => {
          this.shoppingCartService.GetShoppingCart(user.id).subscribe((res: ShoppingCart) => {
            this.priceExclGst = 0;
            this.gst = 0;
            this.priceIncGst = 0;
            this.discount = 0;
            this.oringinalPriceExclGst = 0;
            this.oringinalPriceIncGst = 0;
            this.shoppingCart = res;
            this.dataSource = this.shoppingCart.shoppingItems;
            this.successCheckout = true;
            this.shoppingCartService.MonitorShoppingItemLength(this.dataSource.length);
          },
            (error: any) => {
              console.info(error);
            });
        },
          (error: any) => {
            console.info(error);
          });

        this.shareService.showSuccess(".checkoutbtn", res.message, "right");
      } else {
        this.shareService.showError(".checkoutbtn", res.message, "right");
      }
    },
      (error: any) => {
        console.info(error);
      });
  }

  deleteShoppingItem(shoppingItem: ShoppingItem): void {
    this.shoppingCartService.DeleteShoppingItem(shoppingItem.id).subscribe((res: ApiResponse) => {
      if (res && res.status == "success") {
        this.adminService.GetUserByAccount(_.toNumber(this.shareService.readCookie("userId"))).subscribe((user: User) => {
          this.shoppingCartService.GetShoppingCart(user.id).subscribe((res: ShoppingCart) => {
            this.shoppingCart = res;
            this.dataSource = this.shoppingCart.shoppingItems;
            this.shoppingCartService.MonitorShoppingItemLength(this.dataSource.length);
            if (res && res.shoppingItems.length) {
              this.changePaymentMethod();
            }
          },
            (error: any) => {
              console.info(error);
            });
        },
          (error: any) => {
            console.info(error);
          });
      } else {
        this.shareService.showError(".shoppingItem" + shoppingItem.id, res.message, "right");
      }
    },
      (error: any) => {
        console.info(error);
      });
  }

  changeQuantity(item: ShoppingItem): void {
    if(item.amount<1){
      item.amount = 1;
      this.shareService.showError("#qty"+item.id, "Minimum qty is 1", "right");
    }else{
      this.changePaymentMethod();
    }
    this.shoppingCart.totalItems = 0;
    this.dataSource.forEach(si => {
      this.shoppingCart.totalItems += si.amount;
    });
  }

  changePaymentMethod(): void{
    this.priceExclGst = 0;
    this.gst = 0;
    this.priceIncGst = 0;
    this.discount = 0;
    this.oringinalPriceExclGst = 0;
    this.oringinalPriceIncGst = 0;
    if(this.selectedPayment == "onaccount"){
      this.dataSource.forEach(si => {
        this.oringinalPriceExclGst += si.amount * si.product.originalPrice;
        if (this.role == "agent") {
          si.subTotal = si.amount * si.product.agentPrice;
          this.priceExclGst += si.subTotal;
        } else if (this.role == "wholesaler") {
          si.subTotal = si.amount * si.product.agentPrice;
          this.priceExclGst += si.subTotal;
        }
      });
      this.gst = this.priceExclGst * 0.1;
      this.oringinalPriceIncGst = this.oringinalPriceExclGst + this.oringinalPriceExclGst * 0.1;
      this.priceIncGst = this.gst + this.priceExclGst;
      this.shoppingCart.totalPrice = this.priceIncGst;
      this.discount = this.oringinalPriceIncGst - this.priceIncGst;
    }else{
      this.dataSource.forEach(si => {
        this.oringinalPriceExclGst += si.amount * si.product.originalPrice;
        if (this.role == "agent") {
          si.subTotal = si.amount * si.product.agentPrice - (si.amount * si.product.agentPrice * si.product.prepaymentDiscount/100);
          this.priceExclGst += si.subTotal;
        } else if (this.role == "wholesaler") {
          si.subTotal = si.amount * si.product.wholesalerPrice - (si.amount * si.product.wholesalerPrice * si.product.prepaymentDiscount/100);
          this.priceExclGst += si.subTotal;
        }
      });
      this.gst = this.priceExclGst * 0.1;
      this.oringinalPriceIncGst = this.oringinalPriceExclGst + this.oringinalPriceExclGst * 0.1;
      this.priceIncGst = this.gst + this.priceExclGst;
      this.shoppingCart.totalPrice = this.priceIncGst;
      this.discount = this.oringinalPriceIncGst - this.priceIncGst;
    }
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
