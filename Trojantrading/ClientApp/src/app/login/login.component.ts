import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { UserResponse } from '../models/ApiResponse';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';

declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  userFormGroup: FormGroup;
  sentEmailField: boolean = false;
  email: string;
  showResetText: boolean = false;

  constructor(
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder,
    private nav: NavbarService,
    private shareService: ShareService) {
    this.userFormGroup = this.formBuilder.group({
      account: new FormControl("", Validators.compose([Validators.required])),
      password: new FormControl("", Validators.compose([Validators.required]))
    });
  }

  ngOnInit() {
    this.nav.hide();
    this.userFormGroup.get("account").setValue(this.shareService.readCookie("userName"));
  }

  onSubmit() {
    this.userService.userAuthentication(this.userFormGroup.value).subscribe((data: UserResponse) => {
      this.shareService.createCookie("userToken", data.token, 20);
      this.shareService.createCookie("userName", data.userName, 20);
      this.nav.show();
      this.router.navigate(['/home']);
    },
      (error: any) => {
        console.info(error);
      });
  }

  passwordRecover(): void {
    this.sentEmailField = true;
  }

  backToLogin(): void {
    this.sentEmailField = false;
    this.showResetText = false;
  }

  sendPasswordRecoveryEmail(): void {
    if (this.email) {
      this.showResetText = true;
      this.userService.PasswordRecover(this.email).subscribe((res: UserResponse) => {

      },
        (error: any) => {
          console.info(error);
        });
    } else {
      this.shareService.showError(".sendresetemail", "Please enter your email address", "right");
    }
  }

}
