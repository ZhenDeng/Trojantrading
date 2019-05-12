import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { User } from '../../models/user';

@Component({
  selector: 'app-edit-address',
  templateUrl: './edit-address.component.html',
  styleUrls: ['./edit-address.component.css']
})
export class EditAddressComponent implements OnInit {

  userFormGroup: FormGroup;
  type: string;
  user: User;

  constructor(private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EditAddressComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.userFormGroup = this.formBuilder.group({
      streetNumber: new FormControl(""),
      customerName: new FormControl(""),
      postcode: new FormControl(""),
      addressLine: new FormControl(""),
      suburb: new FormControl(""),
      state: new FormControl("")
    });
  }

  ngOnInit() {
    if(this.data){
      this.type = this.data.type;
      this.user = this.data.user;
      if(this.type == "Billing"){
        this.userFormGroup.get("customerName").setValue(this.data.user.billingCustomerName);
        this.userFormGroup.get("postcode").setValue(this.data.user.billingPostCode);
        this.userFormGroup.get("streetNumber").setValue(this.data.user.billingStreetNumber);
        this.userFormGroup.get("addressLine").setValue(this.data.user.billingAddressLine);
        this.userFormGroup.get("suburb").setValue(this.data.user.billingSuburb);
        this.userFormGroup.get("state").setValue(this.data.user.billingState);
      }else if(this.type == "Shipping"){
        this.userFormGroup.get("customerName").setValue(this.data.user.shippingCustomerName);
        this.userFormGroup.get("streetNumber").setValue(this.data.user.shippingStreetNumber);
        this.userFormGroup.get("postcode").setValue(this.data.user.shippingPostCode);
        this.userFormGroup.get("addressLine").setValue(this.data.user.shippingAddressLine);
        this.userFormGroup.get("suburb").setValue(this.data.user.shippingSuburb);
        this.userFormGroup.get("state").setValue(this.data.user.shippingState);
      }
    }
  }

  updateAddress(): void{
    if(this.type == "Billing"){
      this.user.billingCustomerName = this.userFormGroup.get("customerName").value;
      this.user.billingPostCode = this.userFormGroup.get("postcode").value;
      this.user.billingAddressLine = this.userFormGroup.get("addressLine").value;
      this.user.billingStreetNumber = this.userFormGroup.get("streetNumber").value;
      this.user.billingSuburb = this.userFormGroup.get("suburb").value;
      this.user.billingState = this.userFormGroup.get("state").value;
    }else if(this.type == "Shipping"){
      this.user.shippingCustomerName = this.userFormGroup.get("customerName").value;
      this.user.shippingPostCode = this.userFormGroup.get("postcode").value;
      this.user.shippingStreetNumber = this.userFormGroup.get("streetNumber").value;
      this.user.shippingAddressLine = this.userFormGroup.get("addressLine").value;
      this.user.shippingSuburb = this.userFormGroup.get("suburb").value;
      this.user.shippingState = this.userFormGroup.get("state").value;
    }
    this.dialogRef.close(this.user);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
