import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { ShareService } from '../../services/share.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit {

  userFormGroup: FormGroup;
  roleName: string = "";
  account_validation_messages: any = {
    'account': [
      { class: 'accountValidate', message: 'Please enter user name' }
    ],
    'role': [
      { class: 'roleValidate', message: 'Please choose one role' }
    ],
    'bussinessName': [
      { class: 'bussinessNameValidate', message: 'Please enter business name' }
    ],
    'abn': [
      { class: 'abnValidate', message: 'Please enter Australian Business Number' }
    ],
    'trn': [
      { class: 'trnValidate', message: 'Please enter Tobacco Licence Number' }
    ],
    'email': [
      { class: 'emailValidate', message: 'Please enter valid email' }
    ],
    'status': [
      { class: 'statusValidate', message: 'Please choose one status' }
    ],
    'phone': [
      { class: 'phoneValidate', message: 'Please enter phone number' }
    ]
  }

  constructor(private formBuilder: FormBuilder,
    private shareSevice: ShareService,
    public dialogRef: MatDialogRef<EditUserComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.userFormGroup = this.formBuilder.group({
      account: new FormControl("", Validators.compose([Validators.required])),
      role: this.data&&this.data.user.role.name=="admin"? new FormControl({ value: "admin", disabled: true }, Validators.required):new FormControl("", Validators.compose([Validators.required])),
      bussinessName: new FormControl("", Validators.compose([Validators.required])),
      email: new FormControl("", Validators.compose([Validators.required, Validators.email])),
      phone: new FormControl("", Validators.compose([Validators.required])),
      abn: new FormControl("", Validators.compose([Validators.required])),
      trn: new FormControl("", Validators.compose([Validators.required])),
      status: this.data&&this.data.user.role.name=="admin"? new FormControl({ value: "active", disabled: true }, Validators.required):new FormControl("", Validators.compose([Validators.required])),
      mobile: new FormControl(""),
      billingCustomerName: new FormControl(""),
      billingAddressLine1: new FormControl(""),
      billingAddressLine2: new FormControl(""),
      billingAddressLine3: new FormControl(""),
      billingSuburb: new FormControl(""),
      billingState: new FormControl(""),
      billingPostCode: new FormControl(""),
      shippingCustomerName: new FormControl(""),
      shippingAddressLine1: new FormControl(""),
      shippingAddressLine2: new FormControl(""),
      shippingAddressLine3: new FormControl(""),
      shippingSuburb: new FormControl(""),
      shippingState: new FormControl(""),
      shippingPostCode: new FormControl(""),
      companyAddress: new FormControl(""),
      companyEmail: new FormControl(""),
      companyPhone: new FormControl(""),
      fax: new FormControl(""),
      acn: new FormControl("")
    });
  }

  ngOnInit() {
    if(this.data && this.data.user){
      this.roleName = this.data.user.role;
      this.userFormGroup.get("account").setValue(this.data.user.account);
      this.userFormGroup.get("phone").setValue(this.data.user.phone);
      this.userFormGroup.get("abn").setValue(this.data.user.abn);
      this.userFormGroup.get("trn").setValue(this.data.user.trn);
      this.userFormGroup.get("role").setValue(this.data.user.role);
      this.userFormGroup.get("bussinessName").setValue(this.data.user.bussinessName);
      this.userFormGroup.get("email").setValue(this.data.user.email);
      this.userFormGroup.get("status").setValue(this.data.user.status);
      this.userFormGroup.get("mobile").setValue(this.data.user.mobile);
      this.userFormGroup.get("billingCustomerName").setValue(this.data.user.billingCustomerName);
      this.userFormGroup.get("billingAddressLine1").setValue(this.data.user.billingAddressLine1);
      this.userFormGroup.get("billingAddressLine2").setValue(this.data.user.billingAddressLine2);
      this.userFormGroup.get("billingAddressLine3").setValue(this.data.user.billingAddressLine3);
      this.userFormGroup.get("billingSuburb").setValue(this.data.user.billingSuburb);
      this.userFormGroup.get("billingState").setValue(this.data.user.billingState);
      this.userFormGroup.get("billingPostCode").setValue(this.data.user.billingPostCode);
      this.userFormGroup.get("ShippingCustomerName").setValue(this.data.user.ShippingCustomerName);
      this.userFormGroup.get("ShippingAddressLine1").setValue(this.data.user.ShippingAddressLine1);
      this.userFormGroup.get("ShippingAddressLine2").setValue(this.data.user.ShippingAddressLine2);
      this.userFormGroup.get("ShippingAddressLine3").setValue(this.data.user.ShippingAddressLine3);
      this.userFormGroup.get("ShippingSuburb").setValue(this.data.user.ShippingSuburb);
      this.userFormGroup.get("ShippingState").setValue(this.data.user.ShippingState);
      this.userFormGroup.get("ShippingPostCode").setValue(this.data.user.ShippingPostCode);
      this.userFormGroup.get("companyAddress").setValue(this.data.user.companyAddress);
      this.userFormGroup.get("companyEmail").setValue(this.data.user.companyEmail);
      this.userFormGroup.get("companyPhone").setValue(this.data.user.companyPhone);
      this.userFormGroup.get("fax").setValue(this.data.user.fax);
      this.userFormGroup.get("acn").setValue(this.data.user.acn);
    }
  }

  updateUserDetails(): void {
    if (this.userFormGroup.valid) {
      this.dialogRef.close(this.userFormGroup.getRawValue());
    } else {
      this.isNotValidField('account', this.account_validation_messages.account);
      this.isNotValidField('role', this.account_validation_messages.role);
      this.isNotValidField('phone', this.account_validation_messages.phone);
      this.isNotValidField('abn', this.account_validation_messages.abn);
      this.isNotValidField('trn', this.account_validation_messages.trn);
      this.isNotValidField('bussinessName', this.account_validation_messages.bussinessName);
      this.isNotValidField('email', this.account_validation_messages.email);
      this.isNotValidField('status', this.account_validation_messages.status);
    }
  }

  isNotValidField(path: string, validation: any): void {
    if (!this.userFormGroup.get(path).valid) {
      this.shareSevice.showValidator("." + validation[0].class, validation[0].message, "right", "error");
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
