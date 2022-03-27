import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { RouteData } from "../Models/RouteData";

@Component({
  selector: 'app-report-wrapper',
  templateUrl: './report-wrapper.component.html',
  styleUrls: ['./report-wrapper.component.css']
})
export class ReportWrapperComponent implements OnInit {
  routeData: RouteData = {} as RouteData;

  constructor(
      private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.data.subscribe(x => this.routeData = x as RouteData)
  }
  
  

}
