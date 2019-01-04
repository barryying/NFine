namespace NFine.Domain.Entity.BusinessManage
{
    public class GiftListInsertEntity
    {
        public string F_Id { get; set; }        
        public string EventName { get; set; }
        public string CandidateName { get; set; }
        public decimal? F_Money { get; set; }
        public string F_PaymentStatus { get; set; }
    }
}
