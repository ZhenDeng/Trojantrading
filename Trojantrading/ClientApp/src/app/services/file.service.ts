import { Injectable } from '@angular/core';
import * as _filesaver from 'file-saver';
import * as XLSX from 'xlsx';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { catchError } from 'rxjs/operators';
import { PdfBoard } from '../models/header-info';
import { ApiResponse } from '../models/ApiResponse';


const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
const IMAGE_TYPE = 'image/png';
const PDF_TYPE = 'application/pdf';
const CSV_TYPE = 'application/csv';
const JSON_TYPE = 'application/json';
const EXCEL_EXTENSION = '.xlsx';

@Injectable()
export class FileService {

  base_url: string = "api/PdfBoard";

  constructor(private http: HttpClient,
  ) { }

  GetPdfBoards(): Observable<PdfBoard[]>{
    return this.http.get(this.base_url + "/GetPdfBoards")
    .pipe(catchError(this.handleError));
  }

  SavePdf(type: string, file: any): Observable<ApiResponse>{
    return this.http.post(this.base_url + "/SavePdf?type="+type, file)
    .pipe(catchError(this.handleError));
  }

  DownloadPdf(file: any, fileName: string): Observable<any>{
    return this.http.get(this.base_url + "/DownloadPdf?file="+file+"&fileName="+fileName)
    .pipe(catchError(this.handleError));
  }

  saveFile(url: string): Observable<any> {
    return this.http.get<Blob>(url, { responseType: 'blob' as 'json' })
      .pipe(catchError(this.handleError));
  }

  public exportAsExcelFile(json: any[], excelFileName: string): void {

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(json);

    const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    //const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'buffer' });
    this.saveAsExcelFile(excelBuffer, excelFileName);
  }

  public exportAsCSVFile(json: any[], csvFileName: string): void {

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(json);

    const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };

    const excelBuffer: any = XLSX.write(workbook, { bookType: 'csv', type: 'array' });

    this.saveAsCSVFile(excelBuffer, csvFileName);
  }

  saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], {
      type: EXCEL_TYPE
    });
    _filesaver.saveAs(data, fileName + EXCEL_EXTENSION);
  }

  saveAsImageFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], {
      type: IMAGE_TYPE
    });

    _filesaver.saveAs(data, fileName);
  }

  saveAsPdf(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], {
      type: PDF_TYPE
    });

    _filesaver.saveAs(data, fileName);
  }

  saveAsCSVFile(buffer: any, fileName: string): void {

    const data: Blob = new Blob([buffer], {
      type: CSV_TYPE
    });

    _filesaver.saveAs(data, fileName);
  }

  saveAsJSONFile(buffer: any, fileName: string): void {

    const data: Blob = new Blob([buffer], {
      type: JSON_TYPE
    });

    _filesaver.saveAs(data, fileName);
  }

  private handleError(error: HttpErrorResponse) {
    console.error('server error:', error);
    if (error.error instanceof ErrorEvent) {
      let errMessage = '';
      try {
        errMessage = error.error.message;
      } catch (err) {
        errMessage = error.statusText;
      }
      return Observable.throw(errMessage);
    }
    return Observable.throw(error.error || 'ASP.NET Core server error');
  }
}
