namespace CSIRO.Models
{
    public class Candidate
    {
        public int CandidateID { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }
        public float GPA { get; set; }
        public string CoverLetter { get; set; }

        public int CourseID { get; set; }
        public int UniversityID { get; set; }


    }

}
