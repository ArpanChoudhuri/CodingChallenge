import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Subscription } from 'rxjs/internal/Subscription';
import { MovieSearch } from '../models/moviesearch';



@Component({
  selector: 'movie-search',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.css']
})
export class MovieComponent implements OnInit {
  @Input() form: FormGroup;
  filteredData: MovieSearch = new MovieSearch();
  dataTwo: string = '';

  constructor(private formBuilder: FormBuilder) { }

  get data() {
    return this.form.get('data') as FormControl;
  }
  private dataSub: Subscription;

  ngOnInit() {

    this.form = new FormGroup({
      'data': new FormControl(null),
    });

    this.dataSub = this.data.valueChanges.subscribe(value => {
      console.log('changed', value)
      this.filteredData.title = value
    });

  }

}
