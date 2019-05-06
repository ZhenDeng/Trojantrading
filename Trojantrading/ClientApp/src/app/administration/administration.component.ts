import { FileService } from './../services/file.service';
import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { AdminService } from '../services/admin.service';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { User } from '../models/user';
import * as _ from 'lodash';
import { ApiResponse } from '../models/ApiResponse';
import { ShareService } from '../services/share.service';
import { EditUserComponent } from '../popup-collection/edit-user/edit-user.component';
import { HeadInformation } from '../models/header-info';
import { OrderService } from '../services/order.service';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date';
import { Order } from '../models/order';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css'],
  providers: [DatePipe]
})
export class AdministrationComponent implements OnInit {

  title: string = "Administration";
  displayedColumns: string[] = ["UserName", "BusinessName", "Role", "Email", "Phone", "Status", "EditButton", "DeleteButton"];
  displayedHeaderColumns: string[] = ["Id", "Content", "ImagePath", "EditButton", "DeleteButton"];
  displayedOrderColumns: string[] = ['id', 'customer', 'createdDate', 'totalPrice', 'orderStatus', 'button'];
  dataSource: User[];
  dataSourceFilter: User[];
  dataSourceHeader: HeadInformation[];
  dataSourceHeaderFilter: HeadInformation[];
  role: string;
  dateFrom: NgbDate;
  dateTo: NgbDate;
  strDateFrom: string = '';
  strDateTo: string = '';
  orders: Order[] = [];
  filteredOrder: Order[] = [];
  ordersDataSource = new MatTableDataSource();
  headerDataSource = new MatTableDataSource();

  constructor(
    public nav: NavbarService,
    private adminService: AdminService,
    private shareSevice: ShareService,
    private orderService: OrderService,
    private fileService: FileService,
    private calendar: NgbCalendar,
    public dialog: MatDialog,
    private datePipe: DatePipe
  ) { }

  ngOnInit() {
    this.nav.show();
    this.dateFrom = this.calendar.getNext(this.calendar.getToday(), 'd', -30);
    this.dateTo = this.calendar.getToday();
    this.role = this.shareSevice.readCookie("role");
    this.adminService.GetUsers().subscribe((res: User[]) => {
      if (res) {
        this.dataSource = res;
        this.dataSourceFilter = this.dataSource;
      }
    });

    this.getOrders();
  }

  getOrders() {
    this.convertDateFormat();

    const userId = ''; //admin
    this.orderService.getOrdersByUserID(userId, this.strDateFrom, this.strDateTo).subscribe((value: Order[]) => {
       this.orders = value;
       this.orders.forEach(x => x.customer = x.user.bussinessName);
       this.ordersDataSource = new MatTableDataSource(this.orders);
    },
    (error: any) => {
      console.info(error);
    });
  }

  convertDateFormat(): void {
    this.strDateFrom = this.dateFrom.day + '/' + this.dateFrom.month + '/' + this.dateFrom.year;
    this.strDateTo = this.dateTo.day + '/' + this.dateTo.month + '/' + this.dateTo.year;
  }

  downloadExcel() {
    this.convertDateFormat();

    let exportOrdersArray = this.orders.map(x => ({
      Id: x.id,
      Customer: x.customer,
      CreatedDate: this.datePipe.transform(x.createdDate, 'yyyy-MM-dd'),
      Amount: x.totalPrice,
      Status: x.orderStatus
    }));


    this.fileService.exportAsExcelFile(exportOrdersArray, 'TrojanTrading_Orders_' + this.strDateFrom + '_' + this.strDateTo);

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

  editOrder(order: Order): void {
    //const dialogf = this.dialog.open
  }

  deleteOrder(order: Order):void {
    this.orderService.DeleteOrder(order.id).subscribe((res: ApiResponse) => {
      if (res.status == "success") {
        this.shareSevice.showSuccess("#deleteorder" + order.id, res.message, "right");
        setTimeout(() => {
          this.getOrders();
        }, 2000);
      } else {
        this.shareSevice.showError("#deleteorder" + order.id, res.message, "right");
      }
    },
      (error: any) => {
        console.info(error);
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
    value = value.trim().toLowerCase();
    this.headerDataSource.filter = value;
  }

  applyOrderFilter(value: string) {
    value = value.trim().toLowerCase();
    this.ordersDataSource.filter = value;
  }

  editHeader(element: HeadInformation): void{

  }

  deleteHeader(element: HeadInformation): void{

  }
}
