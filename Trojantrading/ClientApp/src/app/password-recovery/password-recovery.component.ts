import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';
import { ApiResponse } from '../models/ApiResponse';
import * as _ from 'lodash';

@Component({
  selector: 'app-password-recovery',
  templateUrl: './password-recovery.component.html',
  styleUrls: ['./password-recovery.component.css']
})
export class PasswordRecoveryComponent implements OnInit {

  userFormGroup: FormGroup;

  constructor(
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder,
    private nav: NavbarService,
    private shareService: ShareService) {
    this.userFormGroup = this.formBuilder.group({
      // originPassword: new FormControl("", Validators.compose([Validators.required])),
      newPassword: new FormControl("", Validators.compose([Validators.required])),
      confirmPassword: new FormControl("", Validators.compose([Validators.required]))
    });
  }

  ngOnInit() {
    this.nav.hide();
  }

  resetPassword(): void {
    if (this.userFormGroup.valid) {
      this.userService.UpdatePassword(_.toNumber(this.shareService.readCookie("recoverUserId")), this.userFormGroup.get("newPassword").value).subscribe((res: ApiResponse) => {
        if(res && res.status == "success"){
          this.shareService.openSnackBar(res.message, "success");
          this.shareService.createCookie("recoverToken", "", 1);
          this.shareService.createCookie("recoverUser", "", 1);
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 2000);
        }else{
          this.shareService.openSnackBar(res.message, "error");
        }
      },
        (error: any) => {
          console.info(error);
        });
    } else {
      this.shareService.openSnackBar("password can not be empty", "error");
    }
  }
}
