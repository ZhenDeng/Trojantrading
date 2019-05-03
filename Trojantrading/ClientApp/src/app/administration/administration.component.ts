import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { AdminService } from '../services/admin.service';
import { MatDialog } from '@angular/material';
import { User } from '../models/user';
import * as _ from 'lodash';
import { ApiResponse } from '../models/ApiResponse';
import { ShareService } from '../services/share.service';
import { EditUserComponent } from '../popup-collection/edit-user/edit-user.component';
import { HeadInformation } from '../models/header-info';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css']
})
export class AdministrationComponent implements OnInit {

  title: string = "Administration";
  displayedColumns: string[] = ["UserName", "BusinessName", "Role", "Email", "Phone", "Status", "EditButton", "DeleteButton"];
  displayedHeaderColumns: string[] = ["Id", "Content", "ImagePath", "EditButton", "DeleteButton"];
  dataSource: User[];
  dataSourceFilter: User[];
  dataSourceHeader: HeadInformation[];
  dataSourceHeaderFilter: HeadInformation[];
  role: string;

  constructor(
    public nav: NavbarService,
    private adminService: AdminService,
    private shareSevice: ShareService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.nav.show();
    this.role = this.shareSevice.readCookie("role");
    this.adminService.GetUsers().subscribe((res: User[]) => {
      if (res) {
        this.dataSource = res;
        this.dataSourceFilter = this.dataSource;
      }
    });
  }

  addNewUser(): void {
    const dialogRef = this.dialog.open(EditUserComponent, {
      width: '700px',
      data: { type: "Add" }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        let user = {} as User;
        user = result
        this.adminService.AddUser(user).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareSevice.showSuccess(".addnewuser", res.message, "right");
            setTimeout(() => {
              this.adminService.GetUsers().subscribe((res: User[]) => {
                if (res) {
                  this.dataSource = res;
                  this.dataSourceFilter = this.dataSource;
                }
              });
            }, 2000);
          } else {
            this.shareSevice.showError(".addnewuser", res.message, "right");
          }
        },
          (error: any) => {
            console.info(error);
          });
      }
    });
  }

  editUser(user: User): void {
    const dialogRef = this.dialog.open(EditUserComponent, {
      width: '700px',
      data: { user: user, type: "Update" }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        let userModel = {} as User;
        userModel = result;
        userModel.id = user.id;
        userModel.createdDate = user.createdDate;
        userModel.sendEmail = user.sendEmail;
        this.adminService.UpdateUser(userModel).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareSevice.showSuccess("#edit" + user.id, res.message, "right");
            setTimeout(() => {
              this.adminService.GetUsers().subscribe((res: User[]) => {
                if (res) {
                  this.dataSource = res;
                  this.dataSourceFilter = this.dataSource;
                }
              });
            }, 2000);
          } else {
            this.shareSevice.showError("#edit" + user.id, res.message, "right");
          }
        },
          (error: any) => {
            console.info(error);
          });
      }
    });
  }

  deleteUser(user: User): void {
    this.adminService.DeleteUser(user.id).subscribe((res: ApiResponse) => {
      if (res.status == "success") {
        this.shareSevice.showSuccess("#delete" + user.id, res.message, "right");
        setTimeout(() => {
          this.adminService.GetUsers().subscribe((res: User[]) => {
            if (res) {
              this.dataSource = res;
              this.dataSourceFilter = this.dataSource;
            }
          });
        }, 2000);
      } else {
        this.shareSevice.showError("#delete" + user.id, res.message, "right");
      }
    },
      (error: any) => {
        console.info(error);
      });
  }

  applyFilter(value: string): void {
    this.dataSourceFilter = this.dataSource.filter(user => user.account.toLowerCase().trim().includes(value.toLowerCase().trim()) || user.bussinessName.toLowerCase().trim().includes(value.toLowerCase().trim()));
  }

  addNewHeader(): void{

  }

  applyHeaderFilter(value: string): void{

  }

  editHeader(element: HeadInformation): void{

  }

  deleteHeader(element: HeadInformation): void{

  }
}
