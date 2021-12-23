import { Directive, ElementRef, OnInit } from '@angular/core';

@Directive({
  selector: '[appFocus]'
})
export class FocusDirective  implements OnInit{

  constructor(private  el: ElementRef) {
    if(!el.nativeElement['focus']){
      throw new Error('Unfocusable element');
    }
  }

  ngOnInit() {
      const input: HTMLInputElement = this.el.nativeElement as HTMLInputElement;
      input.focus();
      input.select();
  }
}
