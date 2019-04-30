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
  roleName: string;
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
    'email': [
      { class: 'emailValidate', message: 'Please enter valid email' }
    ],
    'status': [
      { class: 'statusValidate', message: 'Please choose one status' }
    ]
  }

  constructor(private formBuilder: FormBuilder,
    private shareSevice: ShareService,
    public dialogRef: MatDialogRef<EditUserComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.userFormGroup = this.formBuilder.group({
      account: new FormControl("", Validators.compose([Validators.required])),
      role: this.data.user.role.name=="admin"? new FormControl({ value: "admin", disabled: true }, Validators.required):new FormControl("", Validators.compose([Validators.required])),
      bussinessName: new FormControl("", Validators.compose([Validators.required])),
      email: new FormControl("", Validators.compose([Validators.required, Validators.email])),
      status: this.data.user.role.name=="admin"? new FormControl({ value: "active", disabled: true }, Validators.required):new FormControl("", Validators.compose([Validators.required]))
    });
  }

  ngOnInit() {
    this.roleName = this.data.user.role.name;
    this.userFormGroup.get("account").setValue(this.data.user.account);
    this.userFormGroup.get("role").setValue(this.data.user.role.name);
    this.userFormGroup.get("bussinessName").setValue(this.data.user.bussinessName);
    this.userFormGroup.get("email").setValue(this.data.user.email);
    this.userFormGroup.get("status").setValue(this.data.user.status);
  }

  updateUserDetails(): void {
    if (this.userFormGroup.valid) {
      this.dialogRef.close(this.userFormGroup.getRawValue());
    } else {
      this.isNotValidField('account', this.account_validation_messages.account);
      this.isNotValidField('role', this.account_validation_messages.role);
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
