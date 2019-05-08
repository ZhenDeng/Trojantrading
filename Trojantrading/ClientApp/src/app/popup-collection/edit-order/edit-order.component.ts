import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ShareService } from './../../services/share.service';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Component, OnInit, Inject } from '@angular/core';

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html',
  styleUrls: ['./edit-order.component.css']
})
export class EditOrderComponent implements OnInit {

  orderFormGroup: FormGroup;
  account_validation_messages: any = {
    'name': [
      { class: 'customerValidate', message: 'Please enter customer name' }
    ],
    'category': [
      { class: 'invoicenoValidate', message: 'Please enter invoice number' }
    ],

  }

  constructor(
    private formBuilder: FormBuilder,
    private shareService: ShareService,
    public dialogRef: MatDialogRef<EditOrderComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.orderFormGroup = this.formBuilder.group({
      customer: new FormControl("", Validators.compose([Validators.required])),
      invoiceNo: new FormControl("", Validators.compose([Validators.required])),
      createdDate: new FormControl("", Validators.compose([Validators.required])),
      orderStatus: new FormControl("")
    });
   }

  ngOnInit() {
    if(this.data.order){
      this.orderFormGroup.get("customer").setValue(this.data.order.customer);
      this.orderFormGroup.get("invoiceNo").setValue(this.data.order.invoiceNo);
      this.orderFormGroup.get("createdDate").setValue(this.data.order.createdDate);
      this.orderFormGroup.get("orderStatus").setValue(this.data.order.orderStatus);
    }
  }


  isNotValidField(path: string, validation: any): void {
    if (!this.orderFormGroup.get(path).valid) {
      this.shareService.showValidator("." + validation[0].class, validation[0].message, "right", "error");
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
