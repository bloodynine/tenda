import { Component } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import { take } from "rxjs/operators";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    title = 'Tenda';
    disableMenu = false;
    disableTransactionMenu = false

    onActivate(outlet: RouterOutlet) {
        outlet.activatedRoute.data.pipe(take(1)).subscribe(x => {
            this.disableMenu = x.disableMenu;
            this.disableTransactionMenu = x.disableTransactionMenu
        })
    }
}
