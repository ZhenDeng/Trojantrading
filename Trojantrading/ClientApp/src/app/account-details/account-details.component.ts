import { Component, OnInit } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { User } from '../models/user';
import { UserAddress } from '../models/UserAddress';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.css']
})
export class AccountDetailsComponent implements OnInit {

  user: User;
  address: UserAddress;
  title: string = "Your Account";
  userFormGroup: FormGroup;
  userPasswordGroup: FormGroup;

  constructor(
    private adminService: AdminService,
    private formBuilder: FormBuilder
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
    this.address = {
      customerName: "admin",
      addressLine1: "asdasd",
      addressLine2: "",
      addressLine3: "",
      suburb: "Kurnell",
      state: "NSW",
      postcode: "2000",
      phone: "22222"
    };
    this.user={
      id: 1,
      account: "admin",
      password: "123",
      bussinessName: "CTC",
      postCode: "2000",
      trn: "222222",
      email: "a@b.c",
      phone: "22222",
      mobile: "222222",
      status: "active",
      sendEmail: true,
      shippingAddress: this.address,
      billingAddress: this.address
    };

    this.userFormGroup.get("trn").setValue(this.user.trn);
    this.userFormGroup.get("email").setValue(this.user.email);
    this.userFormGroup.get("phone").setValue(this.user.phone);
    this.userFormGroup.get("mobile").setValue(this.user.mobile);
    // this.adminService.GetUserByAccount(localStorage.getItem("userName")).subscribe((res: User) => {
    //   this.user = res;
    //   console.info(this.user);
    // },
    //   (error: any) => {
    //     console.info(error);
    //   });
  }

}
