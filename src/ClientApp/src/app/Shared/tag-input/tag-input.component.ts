import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TransactionService } from "../Services/transaction.service";

@Component({
  selector: 'app-tag-input',
  templateUrl: './tag-input.component.html',
  styleUrls: ['./tag-input.component.css']
})
export class TagInputComponent implements OnInit {

  @Input() selectedTags: string[] = [];
  @Output() selectedTagsChange: EventEmitter<string[]> = new EventEmitter<string[]>();
  allTags: string[] = [];
  dropDownTags: string[] = [];
  typedInput: string = "";
  showDropdown: boolean = false;
  activeItemIndex: number = -1;

  constructor(
    private transactionService: TransactionService
  ) {
  }

  ngOnInit(): void {
    if (!this.selectedTags) {
      this.selectedTags = [];
    }
    this.transactionService.GetTags().then(x => this.allTags = x);
  }

  addTag() {
    if (this.typedInput != "" && this.activeItemIndex == -1) {
      this.addAndEmitTag(this.typedInput);
    }
    if (this.activeItemIndex > -1) {
      this.addAndEmitTag(this.dropDownTags[this.activeItemIndex])
    }
  }

  addTagFromDropDown(tag: string) {
    this.addAndEmitTag(tag);
    this.showDropdown = false;
  }

  removeTag(index: number) {
    this.selectedTags.splice(index, 1);
  }

  filterList() {
    if (this.typedInput && this.typedInput != "") {
      this.showDropdown = true;
    }
    const unSelectedTags = this.allTags.filter(x => !this.selectedTags.includes(x))
    this.dropDownTags = unSelectedTags.filter(x => x.toLowerCase().includes(this.typedInput.toLowerCase())).slice(0, 7);
  }

  moveDown() {
    if (this.dropDownTags.length - 1 > this.activeItemIndex) {
      this.activeItemIndex++;
    }
  }

  moveUp() {
    if (this.activeItemIndex > -1) {
      this.activeItemIndex--;
    }
  }

  escape() {
    this.activeItemIndex = -1;
    this.showDropdown = false;
  }

  private addAndEmitTag(tag: string) {
    if (!this.selectedTags) {
      this.selectedTags = [];
    }
    if (this.typedInput != "") {
      this.selectedTags.push(tag)
      this.typedInput = "";
      this.activeItemIndex = -1;
      this.showDropdown = false;
    }
    this.selectedTagsChange.emit(this.selectedTags);
  }
}
