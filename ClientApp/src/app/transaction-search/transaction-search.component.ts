import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Transaction } from "../Shared/Interfaces/Transaction";
import { StateService } from "../state.service";

@Component({
  selector: 'app-transaction-search',
  templateUrl: './transaction-search.component.html',
  styleUrls: ['./transaction-search.component.css']
})
export class TransactionSearchComponent implements OnInit {
  @Input() transactions: Transaction[] = [];
  @Output() id: EventEmitter<string> = new EventEmitter<string>();

  selectionItems: string[] = [];
  typedInput: string = "";
  activeItemIndex: number = -1;

  constructor(
    private stateService: StateService
  ) { }

  ngOnInit(): void {
   this.selectionItems = [...new Set(this.transactions.map(x => x.name))];
  }

  emitId() {
    let selectedId = "";
    if(this.activeItemIndex > -1 && this.activeItemIndex <= this.selectionItems.length){

      selectedId = this.selectionItems[this.activeItemIndex];
    } else {
      selectedId = this.selectionItems.filter(x => x.toLowerCase().includes(this.typedInput.toLowerCase()))[0];
    }
   if(selectedId !== "") {
     this.id.emit(selectedId)
     this.selectionItems = [...new Set(this.transactions.map(x => x.name))];
     this.stateService.CloseQuickSearch();
   }
  }

  selectTransaction(name: string) {
    this.id.emit(name);
  }

  filterList() {
    if(this.typedInput && this.typedInput != "") {
      this.selectionItems = this.selectionItems.filter(x => x.toLowerCase().includes(this.typedInput.toLowerCase()))
    }
  }
  moveDown() {
    if(this.selectionItems.length -1 > this.activeItemIndex){
      this.activeItemIndex++;
    }
  }
  moveUp() {
    if(this.activeItemIndex > -1){
      this.activeItemIndex--;
    }
  }
}
