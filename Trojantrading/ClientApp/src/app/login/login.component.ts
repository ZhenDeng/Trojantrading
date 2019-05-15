import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { UserResponse, ApiResponse } from '../models/ApiResponse';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';
import { MatDialog } from '@angular/material';
import { TermsAndConditionsComponent } from '../popup-collection/terms-and-conditions/terms-and-conditions.component';
import * as _ from 'lodash';

declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {

  userFormGroup: FormGroup;
  userEmailGroup: FormGroup;
  sentEmailField: boolean = false;
  showResetText: boolean = false;
  checked:boolean = false;
  hidePassword: boolean = true;
  loadContent: boolean = false;

  constructor(
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder,
    private nav: NavbarService,
    private shareService: ShareService,
    public dialog: MatDialog,
    private activatedRouter: ActivatedRoute) {
    this.userFormGroup = this.formBuilder.group({
      account: new FormControl("", Validators.compose([Validators.required])),
      password: new FormControl("", Validators.compose([Validators.required]))
    });
  }

  ngOnInit() {
    this.nav.hide();
    this.loadContent = false;
    if(_.toNumber(this.shareService.readCookie("userName"))){
      this.userFormGroup.get("account").setValue(_.toNumber(this.shareService.readCookie("userName")));
    }
    this.loadContent = true;
  }

  onSubmit() {
    this.loadContent = false;
    if(!this.userFormGroup.get('account').valid){
      this.shareService.showError('#account', 'Please enter your account', "right");
    }
    if(!this.userFormGroup.get('password').valid){
      this.shareService.showError('#password', 'Please enter your password', "right");
    }
    if(!this.checked){
      this.shareService.showError('#checkbox', 'Please check the t&c to use our service', "right");
    }

    if(this.userFormGroup.valid && this.checked){
      this.userService.userAuthentication(this.userFormGroup.value).subscribe((data: UserResponse) => {
        if(data && data.token){
          this.loadContent = true;
          this.shareService.createCookie("userToken", data.token, 1);
          this.shareService.createCookie("userName", data.userName.toLowerCase(), 1);
          this.shareService.createCookie("userId", data.userId.toString(), 1);
          this.shareService.createCookie("role", data.role.toLowerCase(), 1);
          this.router.navigate(['/home']);
        }else if(data && data.userName.toLowerCase() == "inactive"){
          this.loadContent = true;
          this.shareService.showError(".loginbtn", "Your Account Has Been Suspended", "right");
        }else if(data && data.userName.toLowerCase() == "wrong"){
          this.loadContent = true;
          this.shareService.showError(".loginbtn", "User name or password is invalid", "right");
        }
      },
        (error: any) => {
          console.info(error);
          this.loadContent = true;
        });
    }
  }

  passwordRecover(): void {
    this.sentEmailField = true;
    this.userEmailGroup = this.formBuilder.group({
      email: new FormControl("", Validators.compose([Validators.required, Validators.email])),
    });
  }

  backToLogin(): void {
    this.sentEmailField = false;
    this.showResetText = false;
  }

  onLoading(currentLoadingStatus: boolean) {
    this.loadContent = !currentLoadingStatus;
  }

  sendPasswordRecoveryEmail(): void {
    if (this.userEmailGroup.get("email").valid) {
      this.userService.ValidateEmail(this.userEmailGroup.get("email").value).subscribe((res: ApiResponse) => {
        if(res && res.status == "success"){
          this.userService.PasswordRecover(this.userEmailGroup.get("email").value, _.toNumber(this.shareService.readCookie("userId"))).subscribe((res: UserResponse) => {
            if(res && res.token){
              this.shareService.createCookie("recoverToken", res.token, 5);
              this.shareService.createCookie("recoverUser", res.userName.toLowerCase(), 5);
              this.shareService.createCookie("recoverUserId", res.userId.toString(), 5);
              this.showResetText = true;
            }
            else{
              this.shareService.showError(".sendresetemail", "Your Account Has Been Suspended", "right");
            }
          },
            (error: any) => {
              console.info(error);
            });
        }
        else{
          this.shareService.showError(".sendresetemail", res.message, "right");
        }
      },
        (error: any) => {
          console.info(error);
        });
    } else {
      this.shareService.showError(".sendresetemail", "Please enter valid email address", "right");
    }
  }

  showTerms(): void{
    this.dialog.open(TermsAndConditionsComponent, {
      width: '800px'
    });
  }
}
