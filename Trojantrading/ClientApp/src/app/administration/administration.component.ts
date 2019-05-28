import { Status } from './../models/order';
import { FileService } from './../services/file.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
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
import { ShoppingCartService } from '../services/shopping-cart.service';
import { ShoppingCart } from '../models/shoppingCart';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import 'rxjs/add/operator/takeUntil';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css'],
  providers: [DatePipe]
})
export class AdministrationComponent implements OnInit, OnDestroy {

  title: string = "Administration";
  displayedColumns: string[] = ["UserName", "Password", "BusinessName", "Role", "Email", "Phone", "Status", "EditButton", "DeleteButton"];
  displayedHeaderColumns: string[] = ["Id", "Content", "ImagePath", "EditButton", "DeleteButton"];
  displayedOrderColumns: string[] = ['invoiceNo', 'customer', 'createdDate', 'totalPrice', 'orderStatus', 'downloadPdf', 'editButton', 'deleteButton'];
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
  statusList: Status[] = [];
  ordersDataSource = new MatTableDataSource();
  headerDataSource = new MatTableDataSource();
  pdfDataSource = new MatTableDataSource();
  loadContent: boolean = false;
  priceExclGst: number;
  gst: number;
  priceIncGst: number;
  discount: number;
  oringinalPriceIncGst: number;
  oringinalPriceExclGst: number;
  
   //unsubscribe
   ngUnsubscribe: Subject<void> = new Subject<void>();
   
  constructor(
    public nav: NavbarService,
    private adminService: AdminService,
    private shareSevice: ShareService,
    private orderService: OrderService,
    private fileService: FileService,
    private calendar: NgbCalendar,
    public dialog: MatDialog,
    private datePipe: DatePipe,
    private headInformationService: HeadInformationService,
    private shoppingCartService: ShoppingCartService,
    private router: Router
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

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  getUsers(): void {
    this.loadContent = false;
    this.adminService.GetUsers().takeUntil(this.ngUnsubscribe)
     .subscribe((res: User[]) => {
      this.dataSource = res;
      this.dataSourceFilter = this.dataSource;
      this.loadContent = true;
    },
      (error: any) => {
        console.info(error);
        this.loadContent = true;
      });
  }

  getOrders() {
    this.convertDateFormat();
    this.statusList = this.orderService.statusList;
    const userId = ''; //admin
    this.loadContent = false;
    this.orderService.getOrdersByUserID(userId, this.strDateFrom, this.strDateTo).takeUntil(this.ngUnsubscribe)
    .subscribe((value: Order[]) => {
      this.orders = value;
      this.ordersDataSource = new MatTableDataSource(this.orders);
      this.loadContent = true;
    },
      (error: any) => {
        console.info(error);
        this.loadContent = true;
      });
  }

  changeStatus(order: Order) {
    this.loadContent = false;
    this.orderService.updateOrder(order).subscribe((res: any) => {
      this.shareSevice.showSuccess(`#status${order.id}`, res.message, "right");
      this.loadContent = true;
    },
      (error: any) => {
        console.info(error);
        this.loadContent = true;
      });
  }

  getHeadInformation(): void {
    this.loadContent = false;
    this.headInformationService.GetHeadInformation().takeUntil(this.ngUnsubscribe)
    .subscribe((res: HeadInformation[]) => {
      this.headerDataSource = new MatTableDataSource(res);
      this.headInformationService.changeMessage(this.headerDataSource.data.length);
      this.loadContent = true;
    },
      (error: any) => {
        console.info(error);
        this.loadContent = true;
      });
  }

  getPdfBoards(): void {
    this.loadContent = false;
    this.fileService.GetPdfBoards().takeUntil(this.ngUnsubscribe)
    .subscribe((res: PdfBoard[]) => {
      this.pdfDataSource = new MatTableDataSource(res);
      this.loadContent = true;
    },
      (error: any) => {
        console.info(error);
        this.loadContent = true;
      });
  }

  convertDateFormat(): void {
    this.strDateFrom = this.dateFrom.day + '/' + this.dateFrom.month + '/' + this.dateFrom.year;
    this.strDateTo = this.dateTo.day + '/' + this.dateTo.month + '/' + this.dateTo.year;
  }

  downloadExcel() {
    this.convertDateFormat();

    let exportOrdersArray = this.orders.map(x => ({
      "Invoice Number": x.invoiceNo,
      "Customer": x.user.bussinessName,
      "CreatedDate": this.datePipe.transform(x.createdDate, 'yyyy-MM-dd'),
      "Total Price Inc GST": x.totalPrice,
      "Payment Method": x.clientMessage,
      "Status": x.orderStatus
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
        this.loadContent = false;
        let user = {} as User;
        user = result
        this.adminService.AddUser(user).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareSevice.showSuccess(".addnewuser", res.message, "right");
            setTimeout(() => {
              this.getUsers();
            }, 1500);
          } else {
            this.loadContent = true;
            this.shareSevice.showError(".addnewuser", res.message, "right");
          }
        },
          (error: any) => {
            console.info(error);
            this.loadContent = true;
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
        this.loadContent = false;
        let orderModel = {} as Order;
        orderModel = result;
        orderModel.id = order.id;
        orderModel.createdDate = order.createdDate;
        this.orderService.updateOrder(orderModel).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareSevice.showSuccess("#editorder" + order.id, res.message, "right");
            setTimeout(() => {
              this.getOrders();
            }, 1500);
          } else {
            this.loadContent = true;
            this.shareSevice.showError("#editorder" + order.id, res.message, "right");
          }
        },
          (error: any) => {
            this.loadContent = true;
            console.info(error);
          });
      }
    });
  }

  deleteOrder(order: Order): void {
    this.loadContent = false;
    this.orderService.DeleteOrder(order.id).subscribe((res: ApiResponse) => {
      if (res.status == "success") {
        this.shareSevice.showSuccess("#deleteorder" + order.id, res.message, "right");
        setTimeout(() => {
          this.getOrders();
        }, 1500);
      } else {
        this.loadContent = true;
        this.shareSevice.showError("#deleteorder" + order.id, res.message, "right");
      }
    },
      (error: any) => {
        this.loadContent = true;
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
        this.loadContent = false;
        let userModel = {} as User;
        userModel = result;
        userModel.id = user.id;
        userModel.createdDate = user.createdDate;
        userModel.sendEmail = user.sendEmail;
        this.adminService.UpdateUser(userModel).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareSevice.showSuccess("#edit" + user.id, res.message, "right");
            setTimeout(() => {
              this.getUsers();
            }, 1500);
          } else {
            this.loadContent = true;
            this.shareSevice.showError("#edit" + user.id, res.message, "right");
          }
        },
          (error: any) => {
            this.loadContent = true;
            console.info(error);
          });
      }
    });
  }

  deleteUser(user: User): void {
    this.loadContent = false;
    this.adminService.DeleteUser(user.id).subscribe((res: ApiResponse) => {
      if (res.status == "success") {
        this.shareSevice.showSuccess("#delete" + user.id, res.message, "right");
        setTimeout(() => {
          this.getUsers();
        }, 1500);
      } else {
        this.loadContent = true;
        this.shareSevice.showError("#delete" + user.id, res.message, "right");
      }
    },
      (error: any) => {
        this.loadContent = true;
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
        this.loadContent = false;
        this.headInformationService.AddHeader(result).subscribe((res: ApiResponse) => {
          if (res.status = "success") {
            this.shareSevice.showSuccess(".addnewhead", res.message, "right");
            setTimeout(() => {
              this.getHeadInformation();
            }, 1500);
          } else {
            this.loadContent = true;
            this.shareSevice.showError(".addnewhead", res.message, "right");
          }
        },
          (error: any) => {
            this.loadContent = true;
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
        this.loadContent = false;
        this.headInformationService.UpdateHeadInfomation(element).subscribe((res: ApiResponse) => {
          if (res && res.status == "success") {
            this.shareSevice.showSuccess("#editheader" + element.id, res.message, "right");
            setTimeout(() => {
              this.getHeadInformation();
            }, 1500);
          } else {
            this.loadContent = true;
            this.shareSevice.showError("#editheader" + element.id, res.message, "right");
          }
        },
          (error: any) => {
            this.loadContent = true;
            console.info(error);
          });
      }
    });
  }

  deleteHeader(element: HeadInformation): void {
    this.loadContent = false;
    this.headInformationService.DeleteHeadInfomation(element).subscribe((res: ApiResponse) => {
      if (res.status == "success") {
        this.shareSevice.showSuccess("#deleteheader" + element.id, res.message, "right");
        setTimeout(() => {
          this.getHeadInformation();
        }, 1500);
      } else {
        this.loadContent = true;
        this.shareSevice.showError("#deleteheader" + element.id, res.message, "right");
      }
    },
      (error: any) => {
        this.loadContent = true;
        console.info(error);
      });
  }

  onLoading(currentLoadingStatus: boolean) {
    this.loadContent = !currentLoadingStatus;
  }

  uploadPdf(): void {
    const dialogRef = this.dialog.open(UploadPdfComponent, {
      width: '650px',
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getPdfBoards();
    });
  }

  uploadUsers(): void {
    const dialogRef = this.dialog.open(UploadUsersComponent, {
      width: '650px',
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getUsers();
    });
  }

  downloadPdf(element: Order): void{
    this.priceExclGst = 0;
    this.gst = 0;
    this.priceIncGst = 0;
    this.discount = 0;
    this.oringinalPriceExclGst = 0;
    this.oringinalPriceIncGst = 0;
    this.loadContent = false;
    this.shoppingCartService.GetCartInIdWithShoppingItems(element.shoppingCartId).subscribe((res: ShoppingCart) => {
      if(res && res.shoppingItems){
        this.adminService.GetUserByAccount(element.userId).subscribe((user: User) => {
          if(res.paymentMethod == "onaccount"){
            res.shoppingItems.forEach(si => {
              this.oringinalPriceExclGst += _.toNumber(si.amount) * si.product.originalPrice;
              if (user.role == "agent") {
                si.subTotal = _.toNumber(si.amount) * si.product.agentPrice;
                this.priceExclGst += si.subTotal;
              } else if (user.role == "wholesaler") {
                si.subTotal = _.toNumber(si.amount) * si.product.wholesalerPrice;
                this.priceExclGst += si.subTotal;
              }
            });
            this.gst = this.priceExclGst * 0.1;
            this.oringinalPriceIncGst = this.oringinalPriceExclGst * 1.1;
            this.priceIncGst = this.gst + this.priceExclGst;
            this.discount = this.oringinalPriceIncGst - this.priceIncGst;
          }else{
            res.shoppingItems.forEach(si => {
              this.oringinalPriceExclGst += _.toNumber(si.amount) * si.product.originalPrice;
              si.subTotal = _.toNumber(si.amount) * si.product.prepaymentDiscount;
              this.priceExclGst += si.subTotal;
            });
            this.gst = this.priceExclGst * 0.1;
            this.oringinalPriceIncGst = this.oringinalPriceExclGst * 1.1;
            this.priceIncGst = this.gst + this.priceExclGst;
            this.discount = this.oringinalPriceIncGst - this.priceIncGst;
          }
          this.fileService.WritePdf(element.id, this.gst, this.priceExclGst, this.discount, element.userId).subscribe((res: ApiResponse) => {
            if(res.status == "success"){
              this.shareSevice.showSuccess("#pdf"+element.id, res.message, "right");
              window.open("/order_"+element.id+".pdf", "_blank");
            }else{
              this.shareSevice.showError("#pdf"+element.id, res.message, "right");
            }
            this.loadContent = true;
          },
            (error: any) => {
              this.loadContent = true;
              console.info(error);
            });
        },
          (error: any) => {
            this.loadContent = true;
            console.info(error);
          });
      }
    },
      (error: any) => {
        this.loadContent = true;
        console.info(error);
      });
  }
}
