import { Component, Input, OnInit } from '@angular/core';
import { ChartTypes } from "../../Models/ChartTypes";

@Component({
  selector: 'app-chart-card',
  templateUrl: './chart-card.component.html',
  styleUrls: ['./chart-card.component.css']
})
export class ChartCardComponent implements OnInit {
  @Input() xAxis = true;
  @Input() yAxis = true;
  @Input() data: any;
  @Input() type: ChartTypes = ChartTypes.None;
  @Input() title: string = '';
  constructor() { }

  ngOnInit(): void {
  }

}
