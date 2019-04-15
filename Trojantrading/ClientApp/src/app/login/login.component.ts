import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { UserResponse, ApiResponse } from '../models/ApiResponse';
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
  userEmailGroup: FormGroup;
  sentEmailField: boolean = false;
  showResetText: boolean = false;

  constructor(
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder,
    private nav: NavbarService,
    private shareService: ShareService) {
    this.userFormGroup = this.formBuilder.group({
      account: new FormControl("", Validators.compose([Validators.required])),
      password: new FormControl("", Validators.compose([Validators.required])),
      role: new FormControl("")
    });

    this.userEmailGroup = this.formBuilder.group({
      email: new FormControl("", Validators.compose([Validators.required, Validators.email])),
    });
  }

  ngOnInit() {
    this.nav.hide();
    this.userFormGroup.get("account").setValue(this.shareService.readCookie("userName"));
  }

  onSubmit() {
    if(this.userFormGroup.valid){
      this.userService.userAuthentication(this.userFormGroup.value).subscribe((data: UserResponse) => {
        this.shareService.createCookie("userToken", data.token, 20);
        this.shareService.createCookie("userName", data.userName, 20);
        this.shareService.createCookie("role", this.userFormGroup.get("role").value, 20);
        this.nav.show();
        this.router.navigate(['/home']);
      },
        (error: any) => {
          console.info(error);
        });
    }else{
      this.shareService.showError(".loginbtn", "Input field can not be empty", "top");
    }
  }

  passwordRecover(): void {
    this.sentEmailField = true;
  }

  backToLogin(): void {
    this.sentEmailField = false;
    this.showResetText = false;
  }

  sendPasswordRecoveryEmail(): void {
    if (this.userEmailGroup.get("email").valid) {
      this.userService.ValidateEmail(this.userEmailGroup.get("email").value).subscribe((res: ApiResponse) => {
        if(res && res.status == "success"){
          console.info("aaa");
          this.userService.PasswordRecover(this.userEmailGroup.get("email").value, this.shareService.readCookie("userName")).subscribe((res: UserResponse) => {
            this.shareService.createCookie("recoverToken", res.token, 5);
            this.shareService.createCookie("recoverUser", res.userName, 5);
            this.showResetText = true;
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
}
