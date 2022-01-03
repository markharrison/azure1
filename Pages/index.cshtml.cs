using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace Azure1.Pages
{
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
    public class IndexModel : PageModel
    {
        private IWebHostEnvironment _env;
        private readonly IMemoryCache _MemoryCache;

        JObject _az1 = null;
        public string strGrid;
        public string strFilter;
        public string strModalInfo;
        public string strTime;

        public IndexModel(IWebHostEnvironment env, IMemoryCache MemoryCache)
        {
            _env = env;
            _MemoryCache = MemoryCache;
        }

        public void doContentSpace()
        {
            strModalInfo += "<br />";
        }

        public void doContentYTvideo(JToken objContentItem)
        {
            string _strText = (objContentItem["text"] == null) ? "" : objContentItem["text"].ToString();
            string _strUrl = (objContentItem["url"] == null) ? "" : objContentItem["url"].ToString();
            _strUrl = "https://www.youtube.com/embed/" + _strUrl /*+ "?&autoplay=1"*/;

            strModalInfo += $"<span class='iconservicemodal' onclick='showLightBox(\"{_strText}\",\"{_strUrl }\");' >";
            strModalInfo += $"<i class='icon-black_fa-videoplay iconfa15xp'></i>{_strText}</span><br/>";
        }

        public void doContentC9video(JToken objContentItem)
        {
            string _strText = (objContentItem["text"] == null) ? "" : objContentItem["text"].ToString();
            string _strUrl = (objContentItem["url"] == null) ? "" : objContentItem["url"].ToString();

            _strUrl = "https://channel9.msdn.com/" + _strUrl + "/player";

            strModalInfo += $"<span class='iconservicemodal' onclick='showLightBox(\"{_strText}\",\"{_strUrl }\");' >";
            strModalInfo += $"<i class='icon-black_fa-videoplay iconfa15xp'></i>{_strText}</span><br/>";
        }

        public void doContentSlide(JToken objContentItem)
        {
            string _strText = (objContentItem["text"] == null) ? "" : objContentItem["text"].ToString();
            string _strUrl = (objContentItem["url"] == null) ? "" : objContentItem["url"].ToString();

            strModalInfo += $"<span class='iconservicemodal' onclick='showLightBox(\"{_strText}\",\"{_strUrl }\");' >";
            strModalInfo += $"<i class='icon-black_fa-powerpoint iconfa15xp'></i>{_strText}</span><br/>";
        }

        public void doContentPDF(JToken objContentItem)
        {
            string _strText = (objContentItem["text"] == null) ? "" : objContentItem["text"].ToString();
            string _strUrl = (objContentItem["url"] == null) ? "" : objContentItem["url"].ToString();
            _strUrl = "https://" + _strUrl;

            strModalInfo += $"<span class='iconservicemodal' onclick='window.open(\"{_strUrl}\",\"_blank\");' > ";
            strModalInfo += $"<i class='icon-black_fa-pdf iconfa15xp'></i>{_strText}</span><br/>";
        }

        public void doContentPPT(JToken objContentItem)
        {
            string _strText = (objContentItem["text"] == null) ? "" : objContentItem["text"].ToString();
            string _strUrl = (objContentItem["url"] == null) ? "" : objContentItem["url"].ToString();
            _strUrl = "https://" + _strUrl;

            strModalInfo += $"<span class='iconservicemodal' onclick='window.open(\"{_strUrl}\",\"_blank\");' > ";
            strModalInfo += $"<i class='icon-black_fa-powerpoint iconfa15xp'></i>{_strText}</span><br/>";
        }

        public void doContentLink(string strUrl, string strText)
        {
            strModalInfo += $"<span class='iconservicemodal' onclick='window.open(\"{strUrl}\",\"_blank\");' > ";
            strModalInfo += $"<i class='icon-black_fa-link iconfa15xp'></i>{strText}</span><br/>";
        }

        public void doContentLinkItem(JToken objContentItem)
        {
            string _strText = (objContentItem["text"] == null) ? "" : objContentItem["text"].ToString();
            string _strUrl = (objContentItem["url"] == null) ? "" : objContentItem["url"].ToString();

            if (_strUrl.Contains("*service*"))
            {
                if (_strText == "") _strText = "Overview";
                _strUrl = _strUrl.Replace("*service*", "https://azure.microsoft.com/services");
            }
            else if (_strUrl.Contains("*feature*"))
            {
                if (_strText == "") _strText = "Overview";
                _strUrl = _strUrl.Replace("*feature*", "https://azure.microsoft.com/features");
            }
            else if (_strUrl.Contains("*docs*"))
            {
                if (_strText == "") _strText = "Documentation";
                _strUrl = _strUrl.Replace("*docs*", "https://docs.microsoft.com/azure");
            }
            else if (_strUrl.Contains("*pricing*"))
            {
                if (_strText == "") _strText = "Pricing";
                _strUrl = _strUrl.Replace("*pricing*", "https://azure.microsoft.com/pricing/details");
            }
            else if (_strUrl.Contains("*ams*"))
            {
                _strUrl = _strUrl.Replace("*ams*", "https://azure.microsoft.com");
            }
            else if (_strUrl.Contains("*ms*"))
            {
                _strUrl = _strUrl.Replace("*ms*", "https://www.microsoft.com");
            }

            if (!_strUrl.StartsWith("http"))
            {
                _strUrl = "https://" + _strUrl;
            }

            doContentLink(_strUrl, _strText);

        }

        public void doAdditonalContent(JArray arrContentItems)
        {
            for (int i = 0; i < arrContentItems.Count; i++)
            {
                string _strItem = arrContentItems[i]["item"].ToString();

                if (_strItem == "link")
                {
                    doContentLinkItem(arrContentItems[i]);
                }
                else if (_strItem == "space")
                {
                    doContentSpace();
                }
                else if (_strItem == "ppt")
                {
                    doContentPPT(arrContentItems[i]);
                }
                else if (_strItem == "pdf")
                {
                    doContentPDF(arrContentItems[i]);
                }
                else if (_strItem == "slide")
                {
                    doContentSlide(arrContentItems[i]);
                }
                else if (_strItem == "YTvideo")
                {
                    doContentYTvideo(arrContentItems[i]);
                }
                if (_strItem == "C9video")
                {
                    doContentC9video(arrContentItems[i]);
                }
                else
                {

                }
            }
        }

        public void doStandardContent(JObject obj)
        {
            string _strOverview = null, _strFeature = null, _strPricing = null, _strDocs = null;
            string _strUrl, _strText;
            bool bSpace = false;


            if (obj["service"] != null)
            {
                _strOverview = obj["service"].ToString();
                _strText = "Overview";
                _strUrl = "https://azure.microsoft.com/services/" + _strOverview;
                doContentLink(_strUrl, _strText);
                bSpace = true;
            }
            else
            {
                if (obj["feature"] != null)
                {
                    _strFeature = obj["feature"].ToString();
                    _strText = "Overview";
                    _strUrl = "https://azure.microsoft.com/features/" + _strFeature;
                    doContentLink(_strUrl, _strText);
                    bSpace = true;
                }
            }

            if (obj["pricing"] != null)
            {
                _strPricing = obj["pricing"].ToString();
                if (_strPricing == "*")
                {
                    if (_strOverview != null)
                        _strPricing = _strOverview;
                    else if (_strFeature != null)
                        _strPricing = _strFeature;
                }
                _strText = "Pricing";
                _strUrl = "https://azure.microsoft.com/pricing/details/" + _strPricing;
                doContentLink(_strUrl, _strText);
                bSpace = true;
            }

            if (obj["docs"] != null)
            {
                _strDocs = obj["docs"].ToString();
                if (_strDocs == "*")
                {
                    if (_strOverview != null)
                        _strDocs = _strOverview;
                    else if (_strFeature != null)
                        _strDocs = _strFeature;
                }
                _strText = "Documentation";
                _strUrl = "https://docs.microsoft.com/azure/" + _strDocs;
                doContentLink(_strUrl, _strText);
                bSpace = true;
            }

            if (bSpace)
            {
                doContentSpace();
            }
        }

        public void doRelated(JArray arrRelatedItems, string strCatNameX)
        {
            string _strTile2 = "";

            for (int i = 0; i < arrRelatedItems.Count; i++)
            {
                string _strRelatedName = arrRelatedItems[i]["name"].ToString();
                string _strRelatedNameX = _strRelatedName.Replace(" / ", "").Replace(" ", "").ToLower();
                string _strIconT;

                if (arrRelatedItems[i]["icon"] != null)
                {
                    _strIconT = arrRelatedItems[i]["icon"].ToString();
                    _strTile2 += $"<div class='tileMnohover tile-{strCatNameX}'>";
                }
                else
                {
                    try
                    {
                        string _strCatNameT = _az1.SelectToken("$..categories[?(@.services[0:].name == '" + _strRelatedName + "')].name").ToString();
                        string _strCatNameTX = _strCatNameT.Replace(" / ", "").Replace(" ", "").ToLower();
                        _strIconT = _az1.SelectToken("$..categories[?(@.services[0:].name == '" + _strRelatedName + "')].services[?(@.name == '" + _strRelatedName + "')].icon").ToString();
                        _strTile2 += $"<div class='tileM tile-{_strCatNameTX}' onclick='showService(\"{ _strRelatedNameX }\");'  >";
                    }
                    catch
                    {
                        _strIconT = "az1ufo";
                        _strTile2 += $"<div class='tileMnohover '>";
                        Debug.WriteLine("Error RelatedName: " + _strRelatedName);
                    }
                }

                _strTile2 += $"<div class='icon-white_az-{_strIconT} tileiconM'></div>";
                _strTile2 += $"<div class='tiletextM'>{_strRelatedName}</div>";
                _strTile2 += "</div>";

            }

            strModalInfo += _strTile2;

        }

        public void doService(JObject obj, string strCatName)
        {
            string _strSrvName = (string)(obj["name"].ToString());
            string _strIcon = (string)(obj["icon"].ToString());
            string _strText = (obj["text"] == null) ? "" : obj["text"].ToString();

            string _strCatNameX = strCatName.Replace(" / ", "").Replace(" ", "").ToLower();
            string _strSrvNameX = _strSrvName.Replace(" ", "").ToLower();

            strGrid += $"<div class='item xxxx-hidden'  onclick='showService(\"{ _strSrvNameX }\");' data-cat='{_strCatNameX}' data-name='{_strSrvName}'>" +
                        $"<div class='item-content'><div class='tile tile-{_strCatNameX}'><div class='icon-white_az-{_strIcon} tileicon'>" +
                        $"</div><div class='tiletext'>{_strSrvName}</div></div></div></div>";

            string _strTile = "";
            _strTile += $"<div class='tilenohover tile-{_strCatNameX}'>";
            _strTile += $"<div class='icon-white_az-{_strIcon} tileicon'></div>";
            _strTile += $"<div class='tiletext'>{_strSrvName}</div>";
            _strTile += "</div>";

            strModalInfo += $"<div id='idSCT-{_strSrvNameX}'>{_strSrvName}</div>'";

            strModalInfo += $"<div id='idSC-{_strSrvNameX}'> ";
            strModalInfo += "<div class='modal-icons'>" + _strTile;

            JArray arrRelatedItems = (JArray)(obj["related"]);
            if (arrRelatedItems != null)
            {
                doRelated(arrRelatedItems, _strCatNameX);
            }

            strModalInfo += "</div><br />";

            if (_strText != "")
                strModalInfo += _strText + "<br /><br />";

            doStandardContent(obj);

            JArray arrContentItems = (JArray)(obj["content"]);
            if (arrContentItems != null)
            {
                doAdditonalContent(arrContentItems);
            }

            strModalInfo += "</div>";

        }

        public void doCategory(JObject obj)
        {
            string _strCatName = (string)(obj["name"].ToString());
            string _strCatNameX = _strCatName.Replace(" / ", "").Replace(" ", "").ToLower();
            string _strIcon = (string)(obj["icon"].ToString());

            strGrid += $"<div class='item' onclick='showCategory(\"{_strCatNameX}\");'  data-cat='category,{_strCatNameX}' data-name='{_strCatName}'><div class='item-content'><div class='tileX tile-{_strCatNameX}'><div class='icon-white_az-{_strIcon} tileicon'>"
                        + $"</div><div class='tiletextX'>{_strCatName}</div></div></div></div>";

            strFilter += $"<li class='tooltipicons' onclick='showCategory(\"{_strCatNameX}\");' data-toggle='tooltip' data-placement='top' title='{_strCatName}'><div class='tileS tile-{_strCatNameX}'>"
                        + $"<div class='icon-white_az-{_strIcon} tileiconS'></div></div></li>";

            JArray arrSrvs = (JArray)obj["services"];
            for (int i = 0; i < arrSrvs.Count; i++)
            {
                doService((JObject)arrSrvs[i], _strCatName);
            }

        }

        public void doGrid()
        {
            var _filesystem = _env.ContentRootPath;
            var _file = System.IO.Path.Combine(_filesystem, "az1data.json");
            _az1 = JObject.Parse(System.IO.File.ReadAllText(_file));

            strGrid = "<div id='idGrid' class='grid ' >";
            strModalInfo = "<div style='display:none;'>";

            strFilter = "<ul class='subnav'>";
            strFilter += $"<li class='tooltipicons' onclick='showOption(\"Home\");' data-toggle='tooltip' data-placement='top' title='Home'><div class='tileS tile-main'><div class='icon-white_az-home tileiconS'></div></div></li>";
            strFilter += $"<li class='tooltipicons' onclick='showOption(\"All\");' data-toggle='tooltip' data-placement='top' title='All Azure Services'><div class='tileS tile-main'><div class='icon-white_az-full tileiconS'></div></div></li>";
            strFilter += $"<li class='tooltipicons' onclick='showOption(\"Search\");' data-toggle='tooltip' data-placement='top' title='Search'><div class='tileS tile-main'><div class='icon-white_az-find tileiconS'></div></div></li>";

            JArray _arrCats = (JArray)_az1["product"]["categories"];
            for (int i = 0; i < _arrCats.Count; i++)
            {
                doCategory((JObject)_arrCats[i]);
            }

            strFilter += "</ul><div id='idSearchBox' style='display:none;'>&nbsp;Search:&nbsp;<input type='text' id='idSearchText' value='' maxlength='30' /></div>";
            strGrid += "</div>";
            strModalInfo += "</div>";
        }

        public void OnGet()
        {
            if (_MemoryCache.TryGetValue("Grid", out strGrid) &&
                _MemoryCache.TryGetValue("Filter", out strFilter) &&
                _MemoryCache.TryGetValue("Time", out strTime) &&
                _MemoryCache.TryGetValue("ModalInfo", out strModalInfo))
            {

                strTime = "Cached " + strTime;
                return;
            }

            doGrid();

            strTime = DateTime.Now.ToString();

            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(24));

            _MemoryCache.Set("Grid", strGrid, options);
            _MemoryCache.Set("Filter", strFilter, options);
            _MemoryCache.Set("ModalInfo", strModalInfo, options);
            _MemoryCache.Set("Time", strTime, options);

            strTime = "Generated " + strTime;

        }
    }
}


