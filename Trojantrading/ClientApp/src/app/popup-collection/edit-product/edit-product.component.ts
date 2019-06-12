import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ShareService } from '../../services/share.service';
import { PackagingList } from '../../models/Product';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit {

  userFormGroup: FormGroup;
  packOne: boolean = false;
  packTwo: boolean = false;
  packagingList: PackagingList[];
  account_validation_messages: any = {
    'itemCode': [
      { class: 'itemCodeValidate', message: 'Please enter item code' }
    ],
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
    'wholesalerPrice': [
      { class: 'wholesalerPriceValidate', message: 'Please enter valid wholesaler price' }
    ]
  }

  constructor(private formBuilder: FormBuilder,
    private shareService: ShareService,
    public dialogRef: MatDialogRef<EditProductComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.userFormGroup = this.formBuilder.group({
      itemCode: new FormControl("", Validators.compose([Validators.required])),
      name: new FormControl("", Validators.compose([Validators.required])),
      category: new FormControl("", Validators.compose([Validators.required])),
      originalPrice: new FormControl("", Validators.compose([Validators.required])),
      agentPrice: new FormControl("", Validators.compose([Validators.required])),
      wholesalerPrice: new FormControl("", Validators.compose([Validators.required])),
      prepaymentDiscount: new FormControl(""),
      status: new FormControl("")
    });
  }

  ngOnInit() {
    this.packOne = false;
    this.packTwo = false;
    if(this.data.product){
      if(this.data.product.packagingLists && this.data.product.packagingLists.length){
        this.data.product.packagingLists.forEach(pack => {
          if(pack.packageName.trim() == "OP"){
            this.packOne = true;
          }
          if(pack.packageName.trim() == "PP"){
            this.packTwo = true;
          }
        });
      }
      this.userFormGroup.get("itemCode").setValue(this.data.product.itemCode);
      this.userFormGroup.get("name").setValue(this.data.product.name);
      this.userFormGroup.get("category").setValue(this.data.product.category);
      this.userFormGroup.get("originalPrice").setValue(this.data.product.originalPrice);
      this.userFormGroup.get("agentPrice").setValue(this.data.product.agentPrice);
      this.userFormGroup.get("wholesalerPrice").setValue(this.data.product.wholesalerPrice);
      this.userFormGroup.get("status").setValue(this.data.product.status);
      this.userFormGroup.get("prepaymentDiscount").setValue(this.data.product.prepaymentDiscount);
    }
  }

  updateProductDetails(): void {
    this.packagingList = [];
    if (this.userFormGroup.valid) {
      if(this.packOne){
        let packagingOne = {
          packageName: "OP",
          productId: this.data.product.id
        } as PackagingList
        this.packagingList.push(packagingOne);
      }
      if(this.packTwo){
        let packagingTwo = {
          packageName: "PP",
          productId: this.data.product.id
        } as PackagingList
        this.packagingList.push(packagingTwo);
      }
      this.dialogRef.close({product: this.userFormGroup.getRawValue(), packaging: this.packagingList});
    } else {
      this.isNotValidField('name', this.account_validation_messages.name);
      this.isNotValidField('itemCode', this.account_validation_messages.itemCode);
      this.isNotValidField('category', this.account_validation_messages.category);
      this.isNotValidField('originalPrice', this.account_validation_messages.originalPrice);
      this.isNotValidField('agentPrice', this.account_validation_messages.agentPrice);
      this.isNotValidField('wholesalerPrice', this.account_validation_messages.wholesalerPrice);
    }
  }

  isNotValidField(path: string, validation: any): void {
    if (!this.userFormGroup.get(path).valid) {
      this.shareService.openSnackBar(validation[0].message, "error");
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
