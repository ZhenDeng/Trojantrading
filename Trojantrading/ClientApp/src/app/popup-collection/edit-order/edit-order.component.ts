import { User } from './../../models/user';
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
  currentUser: User;
  currentRole: string;
  selectedPaymentMethod: string;
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
      clientMessage: new FormControl(""),
      adminMessage: new FormControl(""),
    });
   }

  ngOnInit() {
    if(this.data.order){
      this.getOrdersWithShoppingItems(this.data.order.id);
      this.orderFormGroup.controls["customer"].disable();
      this.orderFormGroup.controls["createdDate"].disable();
      this.orderFormGroup.controls["totalPrice"].disable();
      this.orderFormGroup.get("customer").setValue(this.data.order.user.bussinessName);
      this.orderFormGroup.get("invoiceNo").setValue(this.data.order.invoiceNo);
      this.orderFormGroup.get("createdDate").setValue(this.datePipe.transform(this.data.order.createdDate, 'yyyy-MM-dd'));
      this.orderFormGroup.get("orderStatus").setValue(this.data.order.orderStatus);
      this.orderFormGroup.get("balance").setValue(this.data.order.balance.toFixed(2));
      this.orderFormGroup.get("clientMessage").setValue(this.data.order.clientMessage);
      this.orderFormGroup.get("adminMessage").setValue(this.data.order.adminMessage);
    }
  }

  updateOrderDetails(): void {
    if (this.orderFormGroup.valid) {
      this.currentOrder.invoiceNo = this.orderFormGroup.value.invoiceNo;
      this.currentOrder.orderStatus = this.orderFormGroup.value.orderStatus;
      //this.currentOrder.totalPrice = this.orderFormGroup.value.totalPrice;
      this.currentOrder.balance = this.orderFormGroup.value.balance;
      this.currentOrder.clientMessage = this.orderFormGroup.value.clientMessage;
      this.currentOrder.adminMessage = this.orderFormGroup.value.adminMessage;
      this.currentOrder.shoppingCart.paymentMethod = this.selectedPaymentMethod;
      this.currentCart.totalPrice = this.currentOrder.totalPrice;
      this.currentOrder.shoppingCart = this.currentCart;
      this.currentOrder.shoppingCart.shoppingItems = this.currentItems;

      this.dialogRef.close(this.currentOrder);
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
        //console.info(value);
        this.currentOrder = value;
        this.currentCart = value.shoppingCart;
        this.currentItems = this.currentCart.shoppingItems;
        this.currentUser = value.user;
        this.currentRole = this.currentUser.role;
        this.selectedPaymentMethod = this.currentCart.paymentMethod;

        this.onChangePaymentMethod(this.currentItems, this.selectedPaymentMethod);
    },
    (error: any) => {
      console.info(error);
    });
  }

  onQtyChange(item: ShoppingItem, qty: number) {
    if (!qty) {
      this.shareService.showValidator("#qty" + item.id, "Please enter quantity number", "right", "error");
      item.amount = 1;
    }

    this.onChangePaymentMethod(this.currentItems, this.selectedPaymentMethod);
    
  }

  onChangePaymentMethod(items: ShoppingItem[], paymentMethod: string) {
    this.selectedPaymentMethod = paymentMethod;

    if(paymentMethod == 'onaccount') {
      items.forEach(item => {
        if (this.currentRole == 'agent') {
          item.subTotal = item.product.agentPrice * (item.amount+0.1);
        } else if (this.currentRole == 'wholesaler') {
          item.subTotal = item.product.wholesalerPrice * (item.amount+0.1);
        } else {
          item.subTotal = item.product.originalPrice * (item.amount+0.1);
        }
      })
    } else {
      items.forEach(item => {
        item.subTotal = item.product.prepaymentDiscount * (item.amount+0.1);
      });
    }
    this.currentItems = items;
    this.calculateTotalPrice();
  }

  calculateTotalPrice():void {
    let total = 0;
    this.currentItems.forEach(i => {
       total += i.subTotal;
    });

    this.currentOrder.totalPrice = total;

    this.orderFormGroup.get("totalPrice").setValue(total.toFixed(2));

  }
  
  _keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    let inputChar = String.fromCharCode(event.charCode);

    if (!pattern.test(inputChar)) {
      // invalid character, prevent input
      event.preventDefault();
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
