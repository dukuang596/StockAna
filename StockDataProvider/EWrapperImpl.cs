﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using IBApi;
using Stock.Common;

namespace Stock.DataProvider
{
    public class EWrapperImpl : EWrapper
    {
        EClientSocket _clientSocket;
        private int _nextOrderId;
        // readonly Dictionary<int, List<StockHistoryData>> _reqHistoryDataDictory = new Dictionary<int, List<StockHistoryData>>();
        readonly Dictionary<int, int> _reqStatus = new Dictionary<int, int>();
        //readonly Dictionary<int, List<StockHistoryData>> _reqTickDataDictory = new Dictionary<int, List<StockHistoryData>>();
        private IBDataProvider dp;

        public EWrapperImpl()
        {
            _clientSocket = new EClientSocket(this);
        }

        public EWrapperImpl(IBDataProvider ibpro)
        {
            _clientSocket = new EClientSocket(this);
            dp = ibpro;
        }

        public EClientSocket ClientSocket
        {
            get { return _clientSocket; }
            set { _clientSocket = value; }
        }

        public int NextOrderId
        {
            get { return _nextOrderId; }
            set { _nextOrderId = value; }
        }

        public virtual void error(Exception e)
        {
            Console.WriteLine("Exception thrown: " + e);
            throw e;
        }

        public virtual void error(string str)
        {
            Console.WriteLine("Error: " + str + "\n");
        }

        public virtual void error(int id, int errorCode, string errorMsg)
        {
            if (id >= 10000)
            {
                if (_reqStatus.ContainsKey(id))
                    _reqStatus[id] = 1;
                else
                    _reqStatus.Add(id, 1);
            }

            Console.WriteLine("Error. Id: " + id + ", Code: " + errorCode + ", Msg: " + errorMsg + "\n");
        }

        public virtual void connectionClosed()
        {
            Console.WriteLine("Connection closed.\n");
        }

        public virtual void currentTime(long time)
        {
            Console.WriteLine("Current Time: " + time + "\n");
        }

        public virtual void tickPrice(int tickerId, int field, double price, int canAutoExecute)
        {
            Console.WriteLine("Tick Price. Ticker Id:" + tickerId + ", TickType: " + TickType.getField(field) +
                              ", Price: " + price + ", CanAutoExecute: " + canAutoExecute + "\n");
        }

        public virtual void tickSize(int tickerId, int field, int size)
        {

            Console.WriteLine("Tick Size. Ticker Id:" + tickerId + ", TickType: " + TickType.getField(field) +
                              ", Size: " + size + "\n");
        }

        public virtual void tickString(int tickerId, int tickType, string value)
        {
            Console.WriteLine("Tick string. Ticker Id:" + tickerId + ", TickType: " + TickType.getField(tickType) +
                              ", Value: " + value + "\n");
        }

        public virtual void tickGeneric(int tickerId, int field, double value)
        {
            Console.WriteLine("Tick Generic. Ticker Id:" + tickerId + ", Field: " + field + ", Value: " + value + "\n");
        }

        public virtual void tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints,
            double impliedFuture, int holdDays, string futureExpiry, double dividendImpact, double dividendsToExpiry)
        {
            Console.WriteLine("TickEFP. " + tickerId + ", Type: " + tickType + ", BasisPoints: " + basisPoints +
                              ", FormattedBasisPoints: " + formattedBasisPoints + ", ImpliedFuture: " + impliedFuture +
                              ", HoldDays: " + holdDays + ", FutureExpiry: " + futureExpiry + ", DividendImpact: " +
                              dividendImpact + ", DividendsToExpiry: " + dividendsToExpiry + "\n");
        }

        public virtual void tickSnapshotEnd(int tickerId)
        {
            Console.WriteLine("TickSnapshotEnd: " + tickerId + "\n");
        }

        public virtual void nextValidId(int orderId)
        {
            Console.WriteLine("Next Valid Id: " + orderId + "\n");
            NextOrderId = orderId;
        }

        public virtual void deltaNeutralValidation(int reqId, UnderComp underComp)
        {
            Console.WriteLine("DeltaNeutralValidation. " + reqId + ", ConId: " + underComp.ConId + ", Delta: " +
                              underComp.Delta + ", Price: " + underComp.Price + "\n");
        }

        public virtual void managedAccounts(string accountsList)
        {
            Console.WriteLine("Account list: " + accountsList + "\n");
        }

        public virtual void tickOptionComputation(int tickerId, int field, double impliedVolatility, double delta,
            double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
        {
            Console.WriteLine("TickOptionComputation. TickerId: " + tickerId + ", field: " + field +
                              ", ImpliedVolatility: " + impliedVolatility + ", Delta: " + delta
                              + ", OptionPrice: " + optPrice + ", pvDividend: " + pvDividend + ", Gamma: " + gamma +
                              ", Vega: " + vega + ", Theta: " + theta + ", UnderlyingPrice: " + undPrice + "\n");
        }

        public virtual void accountSummary(int reqId, string account, string tag, string value, string currency)
        {
            Console.WriteLine("Acct Summary. ReqId: " + reqId + ", Acct: " + account + ", Tag: " + tag + ", Value: " +
                              value + ", Currency: " + currency + "\n");
        }

        public virtual void accountSummaryEnd(int reqId)
        {
            Console.WriteLine("AccountSummaryEnd. Req Id: " + reqId + "\n");
        }

        public virtual void updateAccountValue(string key, string value, string currency, string accountName)
        {
            Console.WriteLine("UpdateAccountValue. Key: " + key + ", Value: " + value + ", Currency: " + currency +
                              ", AccountName: " + accountName + "\n");
        }

        public virtual void updatePortfolio(Contract contract, int position, double marketPrice, double marketValue,
            double averageCost, double unrealisedPnl, double realisedPnl, string accountName)
        {
            Console.WriteLine("UpdatePortfolio. " + contract.Symbol + ", " + contract.SecType + " @ " +
                              contract.Exchange
                              + ": Position: " + position + ", MarketPrice: " + marketPrice + ", MarketValue: " +
                              marketValue + ", AverageCost: " + averageCost
                              + ", UnrealisedPNL: " + unrealisedPnl + ", RealisedPNL: " + realisedPnl +
                              ", AccountName: " + accountName + "\n");
        }

        public virtual void updateAccountTime(string timestamp)
        {
            Console.WriteLine("UpdateAccountTime. Time: " + timestamp + "\n");
        }

        public virtual void accountDownloadEnd(string account)
        {
            Console.WriteLine("Account download finished: " + account + "\n");
        }

        public virtual void orderStatus(int orderId, string status, int filled, int remaining, double avgFillPrice,
            int permId, int parentId, double lastFillPrice, int clientId, string whyHeld)
        {
            Console.WriteLine("OrderStatus. Id: " + orderId + ", Status: " + status + ", Filled" + filled +
                              ", Remaining: " + remaining
                              + ", AvgFillPrice: " + avgFillPrice + ", PermId: " + permId + ", ParentId: " + parentId +
                              ", LastFillPrice: " + lastFillPrice + ", ClientId: " + clientId + ", WhyHeld: " + whyHeld +
                              "\n");
        }

        public virtual void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            Console.WriteLine("OpenOrder. ID: " + orderId + ", " + contract.Symbol + ", " + contract.SecType + " @ " +
                              contract.Exchange + ": " + order.Action + ", " + order.OrderType + " " +
                              order.TotalQuantity + ", " + orderState.Status + "\n");
            //clientSocket.reqMktData(2, contract, "", false);
            contract.ConId = 0;
            _clientSocket.placeOrder(_nextOrderId, contract, order);
        }

        public virtual void openOrderEnd()
        {
            Console.WriteLine("OpenOrderEnd");
        }

        public virtual void contractDetails(int reqId, ContractDetails contractDetails)
        {
            Console.WriteLine("ContractDetails. ReqId: " + reqId + " - " + contractDetails.Summary.Symbol + ", " +
                              contractDetails.Summary.SecType + ", ConId: " + contractDetails.Summary.ConId + " @ " +
                              contractDetails.Summary.Exchange + "\n");
        }

        public virtual void contractDetailsEnd(int reqId)
        {
            Console.WriteLine("ContractDetailsEnd. " + reqId + "\n");
        }

        public virtual void execDetails(int reqId, Contract contract, Execution execution)
        {
            Console.WriteLine("ExecDetails. " + reqId + " - " + contract.Symbol + ", " + contract.SecType + ", " +
                              contract.Currency + " - " + execution.ExecId + ", " + execution.OrderId + ", " +
                              execution.Shares + "\n");
        }

        public virtual void execDetailsEnd(int reqId)
        {
            Console.WriteLine("ExecDetailsEnd. " + reqId + "\n");
        }

        public virtual void commissionReport(CommissionReport commissionReport)
        {
            Console.WriteLine("CommissionReport. " + commissionReport.ExecId + " - " + commissionReport.Commission + " " +
                              commissionReport.Currency + " RPNL " + commissionReport.RealizedPNL + "\n");
        }

        public virtual void fundamentalData(int reqId, string data)
        {
            Console.WriteLine("FundamentalData. " + reqId + "" + data + "\n");
        }

        public virtual void historicalData(int reqId, string date, double open, double high, double low, double close,
            int volume, int count, double wap, bool hasGaps)
        {
            //Console.WriteLine("HistoricalData. " + reqId + " - Date: " + date + ", Open: " + open + ", High: " + high + ", Low: " + low + ", Close: " + close + ", Volume: " + volume + ", Count: " + count + ", WAP: " + wap + ", HasGaps: " + hasGaps + "\n");
            if (dp.reqSymbolDict.ContainsKey(reqId))
            {
                var dtask = dp.reqSymbolDict[reqId];
                uint tick = uint.Parse(date);
             
                if (volume > 0&& Util.ConvertFromUtcIntToEst(tick).DayOfYear == dtask.EndDate.DayOfYear)
                   
                        ThreadPool.QueueUserWorkItem(obj =>
                        {
                            try
                            {
                                DataSaver.SaveHistoryData(dtask, tick, open, high, low, close, volume, count, wap, hasGaps);
                            }
                            catch (Exception)
                            {

                                Console.WriteLine("error happened @task：" + dtask.ToString());
                            }
                        }, null);
                   
                  

            }
            //List<StockHistoryData> sp=null;
            //if (_reqHistoryDataDictory.ContainsKey(reqId))
            //    sp = _reqHistoryDataDictory[reqId];
            //else {
            //    sp = new List<StockHistoryData>();
            //    _reqHistoryDataDictory.Add(reqId, sp);
            //}

            //sp.Add(new StockHistoryData { Tick = UInt32.Parse(date), Open = open, High = high, Low = low, Close = close, Volume = volume, Count = count, Wap = wap, HasGaps = hasGaps });

        }

        public virtual void marketDataType(int reqId, int marketDataType)
        {
            Console.WriteLine("MarketDataType. "+reqId+", Type: "+marketDataType+"\n");
        }

        public virtual void updateMktDepth(int tickerId, int position, int operation, int side, double price, int size)
        {
            Console.WriteLine("UpdateMarketDepth. " + tickerId + " - Position: " + position + ", Operation: " + operation + ", Side: " + side + ", Price: " + price + ", Size" + size+"\n");
        }

        public virtual void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, int size)
        {
            Console.WriteLine("UpdateMarketDepthL2. " + tickerId + " - Position: " + position + ", Operation: " + operation + ", Side: " + side + ", Price: " + price + ", Size" + size+"\n");
        }

        
        public virtual void updateNewsBulletin(int msgId, int msgType, String message, String origExchange)
        {
            Console.WriteLine("News Bulletins. "+msgId+" - Type: "+msgType+", Message: "+message+", Exchange of Origin: "+origExchange+"\n");
        }

        public virtual void position(string account, Contract contract, int pos, double avgCost)
        {
            Console.WriteLine("Position. "+account+" - Symbol: "+contract.Symbol+", SecType: "+contract.SecType+", Currency: "+contract.Currency+", Position: "+pos+", Avg cost: "+avgCost+"\n");
        }

        public virtual void positionEnd()
        {
            Console.WriteLine("PositionEnd \n");
        }

        public virtual void realtimeBar(int reqId, long time, double open, double high, double low, double close, long volume, double wap, int count)
        {
            Console.WriteLine("RealTimeBars. " + reqId + " - Time: " + time + ", Open: " + open + ", High: " + high + ", Low: " + low + ", Close: " + close + ", Volume: " + volume + ", Count: " + count + ", WAP: " + wap+"\n");
        }

        public virtual void scannerParameters(string xml)
        {
            Console.WriteLine("ScannerParameters. "+xml+"\n");
        }

        public virtual void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr)
        {
            Console.WriteLine("ScannerData. "+reqId+" - Rank: "+rank+", Symbol: "+contractDetails.Summary.Symbol+", SecType: "+contractDetails.Summary.SecType+", Currency: "+contractDetails.Summary.Currency
                +", Distance: "+distance+", Benchmark: "+benchmark+", Projection: "+projection+", Legs String: "+legsStr+"\n");
        }

        public virtual void scannerDataEnd(int reqId)
        {
            Console.WriteLine("ScannerDataEnd. "+reqId+"\n");
        }

        public virtual void receiveFA(int faDataType, string faXmlData)
        {
            Console.WriteLine("Receing FA: "+faDataType+" - "+faXmlData+"\n");
        }

        public virtual void bondContractDetails(int requestId, ContractDetails contractDetails)
        {
            Console.WriteLine("Bond. Symbol "+contractDetails.Summary.Symbol+", "+contractDetails.Summary);
        }

        public virtual void historicalDataEnd(int reqId, string startDate, string endDate)
        {
            //if (_reqStatus.ContainsKey(reqId))
            //    _reqStatus[reqId] = 1;
            //else
            //    _reqStatus.Add(reqId, 1);
            Console.WriteLine("Time:"+DateTime.Now.ToString(CultureInfo.CurrentCulture)+" Historical data end - "+reqId+" from "+startDate+" to "+endDate);
        }

        //public bool GetHistoryData(int reqId,out IEnumerable<StockHistoryData> historydata) {
        //    historydata = null;
        //    if (_reqStatus.ContainsKey(reqId) && _reqStatus[reqId] == 1)
        //    {
        //        if (_reqHistoryDataDictory.ContainsKey(reqId))
        //        {
        //            historydata = _reqHistoryDataDictory[reqId];
        //            _reqHistoryDataDictory.Remove(reqId);
        //        } 

              
        //        return true;
        //    }
     
        //    return false;
        //}

        public virtual void verifyMessageAPI(string apiData)
        {
            Console.WriteLine("verifyMessageAPI: " + apiData);
        }
        public virtual void verifyCompleted(bool isSuccessful, string errorText)
        {
            Console.WriteLine("verifyCompleted. IsSuccessfule: " + isSuccessful + " - Error: " + errorText);
        }
        public virtual void displayGroupList(int reqId, string groups)
        {
            Console.WriteLine("DisplayGroupList. Request: " + reqId + ", Groups" + groups);
        }
        public virtual void displayGroupUpdated(int reqId, string contractInfo)
        {
            Console.WriteLine("displayGroupUpdated. Request: " + reqId + ", ContractInfo: " + contractInfo);
        }
    }
}
