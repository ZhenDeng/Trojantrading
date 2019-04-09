import { Component, OnInit } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { UserResponse } from '../models/ApiResponse';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.css']
})
export class AccountDetailsComponent implements OnInit {

  user: UserResponse;

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.adminService.GetUserByAccount(localStorage.getItem("userName")).subscribe((res: UserResponse) => {
      this.user = res;
      console.info(this.user);
    },
      (error: any) => {
        console.info(error);
      });
  }

}
