import { Component, OnInit, ViewChild, ViewChildren, AfterViewInit, QueryList, Input } from '@angular/core';
import { MovieService } from '../services/movies.service';
import { Observable, merge, of } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { Movie } from '../models/movie';
import { MovieSearch } from '../models/moviesearch';
import { Library } from '../models/library';
import { ViewOptions } from '../models/view-options';

// import from the folder!!
import { MatSort, Sort } from '@angular/material/sort';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-movies',
  templateUrl: './movieslist.component.html',
  styleUrls: ['./movieslist.component.css']
})
export class MoviesListComponent implements OnInit,AfterViewInit {
  @Input("filteredData") moviesearch: MovieSearch;
  data: Movie[] = [];
  //moviesearch: MovieSearch = new MovieSearch();
  tableColumns: string[] = ['id', 'title', 'year', 'rating', 'franchise', 'date'];
  resultsLength = 0;
  pagesize = 10;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  constructor(private service: MovieService) {
  }

  refresh(options: ViewOptions) {
    this.service.searchMovies(this.moviesearch).subscribe((movies: Movie[]) => {
      console.log('searchmovies', options);

      let data = movies;
      data = data.sort((a, b) => {
        const sortOrder = options.sortDirection === 'asc' ? -1 : 1;
        const valueA = a[options.sortField];
        const valueB = b[options.sortField];

        var result = (valueA < valueB) ? -1 : (valueA > valueB) ? 1 : 0;
        return result * sortOrder;
      });

      const start = options.page * options.pageSize;
      const end = start + options.pageSize;
      data = data.slice(start, end);

      this.resultsLength = movies.length;
      this.data = data;


    })
  }

  ngOnInit() {

      // default data 
      this.refresh(this.getDefaultOptions());
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe((sort: Sort) => {
      console.log('sortChange', this.sort.active);
      this.paginator.pageIndex = 0;
      this.refresh(this.getCurrentOptions());
    });

    this.paginator.page.subscribe((page: PageEvent) => {
      console.log('paginator ', page);
      this.refresh(this.getCurrentOptions());
    });
  }

  getCurrentOptions() {
      const options: ViewOptions = {
        sortField: this.sort.active,
        sortDirection: this.sort.direction,
        page: this.paginator.pageIndex,
        pageSize: this.paginator.pageSize
      };

      return options;
    }

  getDefaultOptions() {
      const options: ViewOptions = {
        sortField: 'name',
        sortDirection: 'asc',
        page: 0,
        pageSize: this.pagesize
      };

      return options;
    }

}
