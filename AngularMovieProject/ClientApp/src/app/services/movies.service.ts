import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

import { Movie } from '../models/movie';
import { Library } from '../models/library';
import { ViewOptions } from '../models/view-options';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { MovieSearch } from '../models/moviesearch';

@Injectable()
export class MovieService {

  private http: HttpClient;
  private baseUrl: string;

  fakeDataLength = 5000;

  constructor(http: HttpClient) {
    this.http = http;
    this.baseUrl = "https://localhost:5001/";
  }

  //findMovies(options: ViewOptions, movieSearch:MovieSearch): Observable<Library> {
  //  let data: any;
  //  console.log('searchmovies', options);
  //   this.searchMovies(movieSearch).subscribe((movie: Movie[]) => {
  //    let data = movie;
  //    data = data.sort((a, b) => {
  //      const sortOrder = options.sortDirection === 'asc' ? -1 : 1;
  //      const valueA = a[options.sortField];
  //      const valueB = b[options.sortField];

  //      var result = (valueA < valueB) ? -1 : (valueA > valueB) ? 1 : 0;
  //      return result * sortOrder;
  //    });

  //    const start = options.page * options.pageSize;
  //    const end = start + options.pageSize;
  //    data = data.slice(start, end);

  //    return of({
  //      items: data,
  //      total: data.length
  //    });
  //  });
    




  //}

  searchMovies(movieSearch: MovieSearch): Observable<Movie[]> {
    return this.http.post(this.baseUrl + 'movie', movieSearch)
      .pipe(
        map(this.extractData)
      );


  }



  randomYear() {
    return Math.floor(Math.random() * 20) + 1990;
  }

  //generateBook(position: number, year: number) {
  //  return <Book>{
  //    name: "Book " + position,
  //    author: "Author " + position,
  //    year: year
  //  };
  //}

  private extractData(res: Movie[]) {
    let body = res;
    return body;
  }

}
