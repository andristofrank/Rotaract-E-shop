import {NgbDate} from "@ng-bootstrap/ng-bootstrap";
import {Injectable} from "@angular/core";
@Injectable(
  { providedIn: 'root'}
)
export class DateFormat {
  constructor() {}

 dateToString( date: NgbDate){
    let month = '' + (date.month);
    let day = '' + date.day;
    const year = '' + date.year;
    if(month.length < 2) { month = '0' + month; }
    if(day.length < 2) { day = '0' + day; }
    return [year, month, day].join('-');
  }
  dateFromString( date: string){
    let dateStr = date.split('-');
    if(dateStr[1].startsWith('0')) {dateStr[1].charAt(1)}
    if(dateStr[2].startsWith('0')) {dateStr[2].charAt(1)}
    let month = parseInt(dateStr[1]);
    let day = parseInt(dateStr[2]);
    const year = parseInt(dateStr[0]);
    return new NgbDate(year, month, day);
  }
}
