import { Status } from './../models/order';
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
import { HeadInformation, PdfBoard } from '../models/header-info';
import { OrderService } from '../services/order.service';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date';
import { Order } from '../models/order';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { DatePipe } from '@angular/common';
import { EditHeaderInfomationComponent } from '../popup-collection/edit-header-infomation/edit-header-infomation.component';
import { HeadInformationService } from '../services/head-information.service';
import { EditOrderComponent } from '../popup-collection/edit-order/edit-order.component';
import { UploadPdfComponent } from '../popup-collection/upload-pdf/upload-pdf.component';
import { UploadUsersComponent } from '../popup-collection/upload-users/upload-users.component';

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
  displayedOrderColumns: string[] = ['invoiceNo', 'customer', 'createdDate', 'totalPrice', 'orderStatus', 'editButton', 'deleteButton'];
  displayedPdfColumns: string[] = ["id", "title", "imagePath"];
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
  statusList: Status[] =[];
  ordersDataSource = new MatTableDataSource();
  headerDataSource = new MatTableDataSource();
  pdfDataSource = new MatTableDataSource();
  loadContent: boolean = false;

  constructor(
    public nav: NavbarService,
    private adminService: AdminService,
    private shareSevice: ShareService,
    private orderService: OrderService,
    private fileService: FileService,
    private calendar: NgbCalendar,
    public dialog: MatDialog,
    private datePipe: DatePipe,
    private headInformationService: HeadInformationService
  ) { }

  ngOnInit() {
    this.nav.show();
    this.dateFrom = this.calendar.getNext(this.calendar.getToday(), 'd', -30);
    this.dateTo = this.calendar.getToday();
    this.role = this.shareSevice.readCookie("role");

    this.getUsers();

    this.getHeadInformation();

    this.getPdfBoards();

    this.getOrders();
  }

  getUsers(): void{
    this.adminService.GetUsers().subscribe((res: User[]) => {
      if (res) {
        this.dataSource = res;
        this.dataSourceFilter = this.dataSource;
      }
    },
      (error: any) => {
        console.info(error);
      });
  }

  getOrders() {
    this.convertDateFormat();
    this.statusList = this.orderService.statusList;

    const userId = ''; //admin
    this.orderService.getOrdersByUserID(userId, this.strDateFrom, this.strDateTo).subscribe((value: Order[]) => {
      this.orders = value;
      this.ordersDataSource = new MatTableDataSource(this.orders);
    },
      (error: any) => {
        console.info(error);
      });
  }

  changeStatus(order: Order) {
    
    this.orderService.updateOrder(order).subscribe((res: any) => {
      this.shareSevice.showSuccess(`#status${order.id}`, res.message, "right");
    },
    (error: any) => {
      console.info(error);
    }); 
  }

  getHeadInformation(): void{
    this.headInformationService.GetHeadInformation().subscribe((res: HeadInformation[]) => {
      if (res) {
        this.headerDataSource = new MatTableDataSource(res);
      }
      this.headInformationService.changeMessage(this.headerDataSource.data.length)
    },
      (error: any) => {
        console.info(error);
      });
  }

  getPdfBoards(): void{
    this.fileService.GetPdfBoards().subscribe((res: PdfBoard[]) => {
      if (res) {
        this.pdfDataSource = new MatTableDataSource(res);
      }
    },
      (error: any) => {
        console.info(error);
      });
  }

  convertDateFormat(): void {
    this.strDateFrom = this.dateFrom.month + '/' + this.dateFrom.day + '/' + this.dateFrom.year;
    this.strDateTo = this.dateTo.month + '/' + this.dateTo.day + '/' + this.dateTo.year;
  }

  downloadExcel() {
    this.convertDateFormat();

    let exportOrdersArray = this.orders.map(x => ({
      Id: x.id,
      Customer: x.user.bussinessName,
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
      const dialogRef = this.dialog.open(EditOrderComponent, {
      width: '700px',
      data: { order: order, statusList: this.orderService.statusList }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        let orderModel = {} as Order;
        orderModel = result;
        orderModel.id = order.id;
        orderModel.createdDate = order.createdDate;

        this.orderService.updateOrder(orderModel).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareSevice.showSuccess("#editorder" + order.id, res.message, "right");
            setTimeout(() => {
              this.getOrders();

            }, 2000);
          } else {
            this.shareSevice.showError("#editorder" + order.id, res.message, "right");
          }
        },
          (error: any) => {
            console.info(error);
          });
      }
    });
  }

  deleteOrder(order: Order): void {
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
          },
            (error: any) => {
              console.info(error);
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

  addNewHeader(): void {
    const dialogRef = this.dialog.open(EditHeaderInfomationComponent, {
      width: '700px',
      data: { type: "Add" }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.headInformationService.AddHeader(result).subscribe((res: ApiResponse) => {
          if(res.status = "success"){
            this.shareSevice.showSuccess(".addnewhead", res.message, "right");
            setTimeout(() => {
              this.getHeadInformation();
            }, 2000);
          }else{
            this.shareSevice.showError(".addnewhead", res.message, "right");
          }
        },
          (error: any) => {
            console.info(error);
          });
      }
    });
  }

  applyHeaderFilter(value: string): void {
    value = value.trim().toLowerCase();
    this.headerDataSource.filter = value;
  }

  applyOrderFilter(value: string) {
    value = value.trim().toLowerCase();
    this.ordersDataSource.filter = value;
  }

  editHeader(element: HeadInformation): void {
    const dialogRef = this.dialog.open(EditHeaderInfomationComponent, {
      width: '700px',
      data: { type: "Update", headInfo: element }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        element.imagePath = result.imagePath;
        element.content = result.content;
        this.headInformationService.UpdateHeadInfomation(element).subscribe((res: ApiResponse) => {
          if (res && res.status == "success") {
            this.shareSevice.showSuccess("#editheader" + element.id, res.message, "right");
            setTimeout(() => {
              this.getHeadInformation();
            }, 2000);
          } else {
            this.shareSevice.showError("#editheader" + element.id, res.message, "right");
          }
        },
          (error: any) => {
            console.info(error);
          });
      }
    });
  }

  deleteHeader(element: HeadInformation): void {
    this.headInformationService.DeleteHeadInfomation(element).subscribe((res: ApiResponse) => {
      if (res.status == "success") {
        this.shareSevice.showSuccess("#deleteheader" + element.id, res.message, "right");
        setTimeout(() => {
          this.getHeadInformation();
        }, 2000);
      } else {
        this.shareSevice.showError("#deleteheader" + element.id, res.message, "right");
      }
    },
      (error: any) => {
        console.info(error);
      });
  }

  onLoading(currentLoadingStatus: boolean) {
    this.loadContent = !currentLoadingStatus;
  }

  uploadPdf(): void{
    this.dialog.open(UploadPdfComponent, {
      width: '650px',
    });
  }

  uploadUsers(): void{
    this.dialog.open(UploadUsersComponent, {
      width: '650px',
    });
  }
}
