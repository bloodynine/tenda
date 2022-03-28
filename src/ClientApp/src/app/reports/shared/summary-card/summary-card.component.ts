import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-summary-card',
  templateUrl: './summary-card.component.html',
  styleUrls: ['./summary-card.component.css']
})
export class SummaryCardComponent implements OnInit {
  @Input() title: string = '';
  @Input() message: string | undefined | null;
  constructor() { }

  ngOnInit(): void {
  }

}
