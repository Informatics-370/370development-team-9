import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import {MatTableModule} from '@angular/material/table';
import {AfterViewInit, ViewChild} from '@angular/core';
import {MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import { Audit } from 'src/app/shared/audit';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-audit',
  templateUrl: './audit.component.html',
  styleUrls: ['./audit.component.scss']
})
export class AuditComponent implements AfterViewInit, OnInit{
  displayedColumns: string[] = ['createdDate', 'user', 'action'];
  dataSource = new MatTableDataSource<Audit>();
  @ViewChild(MatPaginator) paginator: MatPaginator | any;
  @ViewChild(MatSort) sort!: MatSort;
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
  ngOnInit(): void {
    //this.GetAudits();
    this.dataService.GetAudits().subscribe((products:any) => {this.dataSource.data = products});
  }
  constructor( private dataService: DataService) { }

/*   GetAudits()
  {
    this.dataService.GetAudits().subscribe(result => {
      this.audit = result;
      console.log(result);
    })
  } */
}
