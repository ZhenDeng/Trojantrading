import { ShoppingItem } from './../../models/shoppingItem';
import { ShoppingCart } from './../../models/shoppingCart';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ShareService } from './../../services/share.service';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Component, OnInit, Inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { OrderService } from '../../services/order.service';
import { Order } from '../../models/order';

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html',
  styleUrls: ['./edit-order.component.css'],
  providers: [DatePipe]
})
export class EditOrderComponent implements OnInit {

  orderFormGroup: FormGroup;
  currentOrder: Order;
  currentCart: ShoppingCart;
  currentItems: ShoppingItem[] = [];
  account_validation_messages: any = {
    'name': [
      { class: 'customerValidate', message: 'Please enter customer name' }
    ],
    'invoiceNumber': [
      { class: 'invoicenoValidate', message: 'Please enter invoice number' }
    ],

  }

  constructor(
    private formBuilder: FormBuilder,
    private shareService: ShareService,
    private orderService: OrderService,
    private datePipe: DatePipe,
    public dialogRef: MatDialogRef<EditOrderComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.orderFormGroup = this.formBuilder.group({
      customer: new FormControl(""),
      invoiceNo: new FormControl("", Validators.compose([Validators.required])),
      createdDate: new FormControl("", Validators.compose([Validators.required])),
      orderStatus: new FormControl(""),
      totalPrice: new FormControl(""),
      balance: new FormControl(""),
      note: new FormControl(""),
      clientMessage: new FormControl(""),
      adminMessage: new FormControl(""),
      paymentMethod: new FormControl(""),
    });
   }

  ngOnInit() {
    if(this.data.order){
      this.getOrdersWithShoppingItems(this.data.order.id);
      this.orderFormGroup.controls["customer"].disable();
      this.orderFormGroup.controls["createdDate"].disable();
      this.orderFormGroup.get("customer").setValue(this.data.order.user.bussinessName);
      this.orderFormGroup.get("invoiceNo").setValue(this.data.order.invoiceNo);
      this.orderFormGroup.get("createdDate").setValue(this.datePipe.transform(this.data.order.createdDate, 'yyyy-MM-dd'));
      this.orderFormGroup.get("orderStatus").setValue(this.data.order.orderStatus);
      this.orderFormGroup.get("totalPrice").setValue(this.data.order.totalPrice.toFixed(2));
      this.orderFormGroup.get("balance").setValue(this.data.order.balance.toFixed(2));
      this.orderFormGroup.get("clientMessage").setValue(this.data.order.clientMessage);
      this.orderFormGroup.get("adminMessage").setValue(this.data.order.adminMessage);
    }
  }

  updateOrderDetails(): void {
    if (this.orderFormGroup.valid) {
      this.currentOrder.invoiceNo = this.orderFormGroup.value.invoiceNo;
      this.currentOrder.orderStatus = this.orderFormGroup.value.orderStatus;
      this.currentOrder.totalPrice = this.orderFormGroup.value.totalPrice;
      this.currentOrder.balance = this.orderFormGroup.value.balance;
      this.currentOrder.clientMessage = this.orderFormGroup.value.clientMessage;
      this.currentOrder.adminMessage = this.orderFormGroup.value.adminMessage;
      console.log(this.currentOrder);
      //this.dialogRef.close(this.orderFormGroup.value);
    } else {
      this.isNotValidField('name', this.account_validation_messages.name);
      this.isNotValidField('invoiceNo', this.account_validation_messages.invoiceNumber);

    }
  }


  isNotValidField(path: string, validation: any): void {
    if (!this.orderFormGroup.get(path).valid) {
      this.shareService.showValidator("." + validation[0].class, validation[0].message, "right", "error");
    }
  }

  getOrdersWithShoppingItems(id: number) {
      
    this.orderService.getOrdersWithShoppingItems(id).subscribe((value: Order) => {
        this.currentOrder = value;
        this.currentCart = value.shoppingCart;
        this.currentItems = this.currentCart.shoppingItems;

        this.orderFormGroup.get("note").setValue(this.currentCart.note);
        this.orderFormGroup.get("paymentMethod").setValue(this.currentCart.paymentMethod);

        
    },
    (error: any) => {
      console.info(error);
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
