import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {HubConnectionBuilder, IHttpConnectionOptions, LogLevel} from "@microsoft/signalr";
import {BehaviorSubject, timer} from "rxjs";

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {
  total: BehaviorSubject<Seed> = new BehaviorSubject<Seed>({amount: 0 } as Seed);
  bah: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  foo: Seed = {} as Seed;
  stupid = 0;
  constructor(private http: HttpClient, private cdef: ChangeDetectorRef) { }
  options: IHttpConnectionOptions = {
    accessTokenFactory(): string | Promise<string> {
      return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VybmFtZSI6InRlc3QxIiwiVXNlcklkIjoiNjE5NTI3M2IzYjJlZWEyYjVlYWZhYzk4IiwiU2VlZElkIjoiNjE5NTBiZDg2MTFhMmMwNmMwNmVjYjE1IiwiTmFtZSI6IjYxOTUyNzNiM2IyZWVhMmI1ZWFmYWM5OCIsIm5iZiI6MTYzODM2Nzk5NywiZXhwIjoxNjM4NDU0Mzk3LCJpYXQiOjE2MzgzNjc5OTd9.NkV4tgSaRktAISol8cEY_-nq49QwNW-1XDgXm3SxzXI";
    }
  }

  ngOnInit(): void {
    const conn = new HubConnectionBuilder()
      .configureLogging(LogLevel.Information)
      .withUrl("https://localhost:7139/api/resolvedTotal", this.options)
      .build();
    conn.start().then(x => {
      console.log('Sig Connected')
    }).catch(x => console.error(x.toString()));

    conn.on("Foo", (x: Seed) => {
      timer(200).subscribe(y => {
        this.total.next(x);
        this.bah.next(Math.random());
      })

    });

    this.total.subscribe(x => {
      this.foo = x;
      this.cdef.detectChanges()
    });
  }
}
