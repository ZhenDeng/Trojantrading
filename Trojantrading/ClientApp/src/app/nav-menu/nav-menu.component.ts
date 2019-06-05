import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ShareService } from '../services/share.service';
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { AdminService } from '../services/admin.service';
import { User } from '../models/user';
import { ShoppingCart } from '../models/shoppingCart';
import { ShoppingItem } from '../models/shoppingItem';
import { ApiResponse } from '../models/ApiResponse';
import * as _ from 'lodash';
import { Category } from '../models/Product';
import { ProductService } from '../services/product.service';
import { HeadInformationService } from '../services/head-information.service';
import { HeadInformation } from '../models/header-info';
import { DeleteConfirmComponent } from '../popup-collection/delete-confirm/delete-confirm.component';
import { MatDialog } from '@angular/material';

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
  headerDataSource: HeadInformation[] = [];
  filePath: string;
  inCartPage: boolean = false;

  constructor(
    public nav: NavbarService,
    private router: Router,
    private activatedRouter: ActivatedRoute,
    private shareService: ShareService,
    private shoppingCartService: ShoppingCartService,
    private adminService: AdminService,
    private productService: ProductService,
    private headInformationService: HeadInformationService,
    private dialog: MatDialog
  ) {
  }

  ngOnInit() {
    this.role = this.shareService.readCookie("role");
    this.filePath = this.role.toUpperCase() + " PRICE LIST.pdf";
    this.categoryList = this.productService.categoryList;

    this.headInformationService.GetHeadInformation().subscribe((res: HeadInformation[]) => {
      if (res) {
        this.headerDataSource = res;
      }
    },
      (error: any) => {
        console.info(error);
      });

    this.activatedRouter.url.subscribe((url: any) => {
      if (url && url[0]) {
        if (url[0].path == "cart") {
          this.inCartPage = true;
        } else {
          this.inCartPage = false;
        }
      }
    },
      (error: any) => {
        console.info(error);
      })

    this.shoppingCartService.currentShoppingItemLength.subscribe((length: number) => {
      this.shoppingCartService.AddShoppingCart(_.toNumber(this.shareService.readCookie("userId"))).subscribe((sc: ApiResponse) => {
        this.shoppingCartService.GetShoppingCart(_.toNumber(this.shareService.readCookie("userId"))).subscribe((res: ShoppingCart) => {
          if (res && res.shoppingItems.length) {
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
    const dialogRef = this.dialog.open(DeleteConfirmComponent, {
      width: '500px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
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
            this.shareService.openSnackBar(res.message, "error");
          }
        },
          (error: any) => {
            console.info(error);
          });
      }
    });

  }

  continueShopping(): void {
    this.router.navigate(['/home']);
  }

  logOut(): void {
    this.shareService.savecookies("userToken", "", 1);
    this.shareService.savecookies("userName", "", 1);
    this.shareService.savecookies("userId", "", 1);
    this.shareService.savecookies("role", "", 1);
    this.router.navigate(['login'], { relativeTo: this.activatedRouter.parent })
  }

}
