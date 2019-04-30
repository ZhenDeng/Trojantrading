import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ShareService } from '../../services/share.service';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit {

  userFormGroup: FormGroup;
  account_validation_messages: any = {
    'name': [
      { class: 'descriptionValidate', message: 'Please enter description' }
    ],
    'category': [
      { class: 'categoryValidate', message: 'Please choose one category' }
    ],
    'originalPrice': [
      { class: 'originalPriceValidate', message: 'Please enter valid original price' }
    ],
    'agentPrice': [
      { class: 'agentPriceValidate', message: 'Please enter valid agent price' }
    ],
    'resellerPrice': [
      { class: 'resellerPriceValidate', message: 'Please enter valid reseller price' }
    ]
  }

  constructor(private formBuilder: FormBuilder,
    private shareSevice: ShareService,
    public dialogRef: MatDialogRef<EditProductComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.userFormGroup = this.formBuilder.group({
      name: new FormControl("", Validators.compose([Validators.required])),
      category: new FormControl("", Validators.compose([Validators.required])),
      originalPrice: new FormControl("", Validators.compose([Validators.required])),
      agentPrice: new FormControl("", Validators.compose([Validators.required])),
      resellerPrice: new FormControl("", Validators.compose([Validators.required])),
      status: new FormControl("")
    });
  }

  ngOnInit() {
    if(this.data.product){
      this.userFormGroup.get("name").setValue(this.data.product.name);
      this.userFormGroup.get("category").setValue(this.data.product.category);
      this.userFormGroup.get("originalPrice").setValue(this.data.product.originalPrice);
      this.userFormGroup.get("agentPrice").setValue(this.data.product.agentPrice);
      this.userFormGroup.get("resellerPrice").setValue(this.data.product.resellerPrice);
      this.userFormGroup.get("status").setValue(this.data.product.status);
    }
  }

  updateProductDetails(): void {
    if (this.userFormGroup.valid) {
      this.dialogRef.close(this.userFormGroup.value);
    } else {
      this.isNotValidField('name', this.account_validation_messages.name);
      this.isNotValidField('category', this.account_validation_messages.category);
      this.isNotValidField('originalPrice', this.account_validation_messages.originalPrice);
      this.isNotValidField('agentPrice', this.account_validation_messages.agentPrice);
      this.isNotValidField('resellerPrice', this.account_validation_messages.resellerPrice);
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
