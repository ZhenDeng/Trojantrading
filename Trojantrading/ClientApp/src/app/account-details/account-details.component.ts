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
import { ApiResponse } from '../models/ApiResponse';

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
    this.adminService.GetUserWithAddress(this.shareService.readCookie("userName")).subscribe((res: User) => {
      this.user = res;
      this.userFormGroup.get("trn").setValue(this.user.trn);
      this.userFormGroup.get("email").setValue(this.user.email);
      this.userFormGroup.get("phone").setValue(this.user.phone);
      this.userFormGroup.get("mobile").setValue(this.user.mobile);
    },
      (error: any) => {
        console.info(error);
      });
  }

  updateInfo(): void {
    if (!this.userFormGroup.get("trn").valid) {
      this.shareService.showError(".trn", "Tobacco Licence Number can not be empty", "right");
    }
    if (!this.userFormGroup.get("email").valid) {
      this.shareService.showError(".email", "Email can not be empty", "right");
    }
    if (!this.userFormGroup.get("phone").valid) {
      this.shareService.showError(".phone", "Phone can not be empty", "right");
    }
    if (!this.userFormGroup.get("mobile").valid) {
      this.shareService.showError(".mobile", "Mobile can not be empty", "right");
    }
    if (this.userFormGroup.valid) {
      this.user.trn = this.userFormGroup.get("trn").value;
      this.user.email = this.userFormGroup.get("email").value;
      this.user.phone = this.userFormGroup.get("phone").value;
      this.user.mobile = this.userFormGroup.get("mobile").value;
      this.adminService.UpdateUser(this.user).subscribe((res: ApiResponse) => {
        if (res && res.status == "success") {
          this.shareService.showSuccess(".updateinfo", res.message, "right");
        } else {
          this.shareService.showError(".updateinfo", res.message, "right");
        }
      },
        (error: any) => {
          console.info(error);
        });
    }
  }

  updatePassword(): void {

  }

  validateOldPassword(): void {
    
  }

  backToProduct(): void {
    this.router.navigate(["/home"]);
  }
}
