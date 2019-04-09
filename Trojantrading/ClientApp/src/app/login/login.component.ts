import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { UserResponse } from '../models/ApiResponse';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  userFormGroup: FormGroup;
  showNav:string = 'login';

  constructor(
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder) {
    this.userFormGroup = this.formBuilder.group({
      account: new FormControl("", Validators.compose([Validators.required])),
      password: new FormControl("", Validators.compose([Validators.required]))
    });
  }

  ngOnInit() {
    this.userFormGroup.get("account").setValue(localStorage.getItem("userName"));
  }

  onSubmit() {
    this.userService.userAuthentication(this.userFormGroup.value).subscribe((data: UserResponse) => {
      localStorage.setItem('userToken', data.token);
      localStorage.setItem('userName', data.userName);
      this.router.navigate(['/home'])
    },
      (error: any) => {
        console.info(error);
      });
  }

}
