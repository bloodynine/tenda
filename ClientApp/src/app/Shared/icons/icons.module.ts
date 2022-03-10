import { NgModule } from '@angular/core';

import { FeatherModule } from 'angular-feather';
import {Square, CheckSquare, PlusSquare, Repeat, Plus, X, XCircle, Search} from 'angular-feather/icons';

const icons = {
  Square, PlusSquare, CheckSquare, Repeat, Plus, X, XCircle, Search
};

@NgModule({
  imports: [
    FeatherModule.pick(icons)
  ],
  exports: [
    FeatherModule
  ]
})
export class IconsModule { }
