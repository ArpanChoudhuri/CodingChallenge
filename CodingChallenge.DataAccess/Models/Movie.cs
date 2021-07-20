using System;
using System.Runtime.Serialization;

namespace CodingChallenge.DataAccess.Models
{
    [DataContract (Name = "Movie")]
    public class Movie : IEquatable<Movie>
    {
        [DataMember(Name = "ID", Order = 1)]
        public int ID { get; set; }
        [DataMember(Name = "Title", Order = 2)]
        public string Title { get; set; }
        [DataMember(Name = "Year", Order = 3)]
        public int Year { get; set; }
        [DataMember(Name = "Rating", Order = 4)]
        public double Rating { get; set; }
        [DataMember(Name = "Franchise", Order = 5)]
        public string Franchise { get; set; }

        [DataMember(Name = "Date", Order = 6)]
        public DateTime? Date { get; set; }

        public bool Equals(Movie other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the movie properties are equal.
            return Title.Equals(other.Title) ;
        }
        public override int GetHashCode()
        {

            //Get hash code for the Title field if it is not null.
            int hashProductName = Title == null ? 0 : Title.GetHashCode();

            //Calculate the hash code for the movies.
            return hashProductName ^ hashProductName;
        }
    }
}
