import { Component, OnInit } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { User } from '../models/user';
import { UserAddress } from '../models/UserAddress';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ShoppingItem } from '../models/shoppingItem';
import { Order } from '../models/order';
import { ShoppingCart } from '../models/shoppingCart';
import { ShareService } from '../services/share.service';
import { Router } from '@angular/router';
import { NavbarService } from '../services/navbar.service';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.css']
})
export class AccountDetailsComponent implements OnInit {

  user: User;
  address: UserAddress;
  shoppingCart: ShoppingCart;
  title: string = "Your Account";
  userFormGroup: FormGroup;
  userPasswordGroup: FormGroup;
  shoppingItems: ShoppingItem[];
  orders: Order[];

  constructor(
    private adminService: AdminService,
    private formBuilder: FormBuilder,
    private shareService: ShareService,
    private router: Router,
    public nav: NavbarService
    ) { 
    this.userFormGroup = this.formBuilder.group({
      trn: new FormControl("", Validators.compose([Validators.required])),
      email: new FormControl("", Validators.compose([Validators.required])),
      phone: new FormControl("", Validators.compose([Validators.required])),
      mobile: new FormControl(""),
    });

    this.userPasswordGroup = this.formBuilder.group({
      password: new FormControl("", Validators.compose([Validators.required])),
      newpassord: new FormControl("", Validators.compose([Validators.required])),
      confirmpassord: new FormControl("", Validators.compose([Validators.required]))
    });
  }

  ngOnInit() {
    this.nav.hideTab();
    this.adminService.GetUserByAccount(this.shareService.readCookie("userName")).subscribe((res: User) => {
      this.user = res;
      console.info(this.user);
      this.userFormGroup.get("trn").setValue(this.user.trn);
      this.userFormGroup.get("email").setValue(this.user.email);
      this.userFormGroup.get("phone").setValue(this.user.phone);
      this.userFormGroup.get("mobile").setValue(this.user.mobile);
    },
      (error: any) => {
        console.info(error);
      });

    this.adminService.GetShippingAddress(1).subscribe((res: UserAddress) => {
      console.info(res);
    });
  }

  backToProduct(): void{
    this.router.navigate(["/home"]);
  }
}
