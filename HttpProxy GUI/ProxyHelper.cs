using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Win32;
using Eavesdrop;
using System.Net;

namespace HttpProxy.GUI
{
    internal static class ProxyHelper
    {
        static 日志记录器 c = new 日志记录器("Proxy");


        static ProxyHelper()
        {
            Eavesdropper.Certifier = new Certifier("Cyt", "HttpProxy Root Certificate Authority")
            {
                NotBefore = DateTime.Now,
                NotAfter = DateTime.Now.AddMonths(1)
            };
        }

        #region - Windows API -

        [DllImport("wininet.dll")]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        private const int INTERNET_OPTION_REFRESH = 37;

        #endregion

        #region - System Proxy -

        private const string RegKeyInternetSettings = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
        private const string RegProxyEnable = "ProxyEnable";
        private const string RegProxyServer = "ProxyServer";
        private const string RegProxyOverride = "ProxyOverride";

        private const string PassBy =
            "localhost;127.*;10.*;172.16.*;172.17.*;172.18.*;172.19.*;172.20.*;172.21.*;172.22.*;172.23.*;172.24.*;172.25.*;172.26.*;172.27.*;172.28.*;172.29.*;172.30.*;172.31.*;192.168.*";

        private static void FlushOs()
        {
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        private static bool _isSetSystemProxy;

        private static void SetSystemProxy(int proxyPort)
        {
            using (var reg = Registry.CurrentUser.OpenSubKey(RegKeyInternetSettings, true))
            {
                reg.SetValue(RegProxyServer, $"http=127.0.0.1:{proxyPort};https=127.0.0.1:{proxyPort}");
                reg.SetValue(RegProxyOverride, PassBy);
                reg.SetValue(RegProxyEnable, 1);
            }

            _isSetSystemProxy = true;
            FlushOs();
        }

        private static void CloseSystemProxy()
        {
            if (!_isSetSystemProxy) return;
            _isSetSystemProxy = false;

            using (var reg = Registry.CurrentUser.OpenSubKey(RegKeyInternetSettings, true))
                reg.SetValue(RegProxyEnable, 0);
            FlushOs();
        }

        #endregion

        #region - GS Proxy Server -

        private const string ProxyOverrides =
            "localhost;1*;" + //" 127.*;10.*;192.168.*;" +
            "*0;*1;*2;*3;*4;*5;*6;*7;*8;*9;" +
            "*q;*w;*e;*r;*t;*y;*u;*i;*o;*p;*a;*s;*d;*f;*g;*h;*j;*k;*l;*z;*x;*c;*v;*b;*n" +
            "*q.com;*w.com;*t.com;*y.com;*u.com;*i.com;*p.com;*a.com;*d.com;*f.com;*g.com;*h.com;*j.com;*k.com;*l.com;*z.com;*x.com;*c.com;*v.com;*b.com;*m.com;" +
            //米哈游域名处理
            //"*e.com;*s.com;*r.com;*n.com;" +
            "*qr.com;*wr.com;*er.com;*rr.com;*tr.com;*yr.com;*ur.com;*iq.com;*oq.com;*pq.com;*aq.com;*dq.com;*fq.com;*gq.com;*hq.com;*jq.com;*kq.com;*lq.com;*zq.com;*xq.com;*cq.com;*vq.com;*bq.com;*nq.com;*mq.com;" +
            "*qs.com;*ws.com;*es.com;*rs.com;*ts.com;*ys.com;*us.com;*is.com;*os.com;*ps.com;*as.com;*ss.com;*ds.com;*fs.com;*gs.com;*hs.com;*js.com;*ks.com;*zs.com;*xs.com;*cs.com;*vs.com;*bs.com;*ns.com;*ms.com" +
            "*qe.com;*we.com;*ee.com;*re.com;*te.com;*ye.com;*ue.com;*ie.com;*oe.com;*pe.com;*ae.com;*de.com;*fe.com;*ge.com;*he.com;*je.com;*ke.com;*le.com;*ze.com;*xe.com;*ce.com;*ve.com;*be.com;*ne.com;*me.com" +
            "*qn.com;*wn.com;*rn.com;*tn.com;*yn.com;*un.com;*in.com;*on.com;*pn.com;*an.com;*sn.com;*dn.com;*fn.com;*gn.com;*hn.com;*jn.com;*kn.com;*ln.com;*zn.com;*xn.com;*cn.com;*vn.com;*bn.com;*nn.com;*mn.com;" +
            "*qo.com;*wo.com;*eo.com;*ro.com;*to.com;*uo.com;*io.com;*oo.com;*po.com;*ao.com;*so.com;*do.com;*fo.com;*go.com;*ho.com;*jo.com;*ko.com;*lo.com;*zo.com;*xo.com;*co.com;*vo.com;*bo.com;*no.com;*mo.com;";
        //常用域名处理
        //"*bing*;*google*;*live.com;*office.com;*weibo*;*yahoo*;*taobao*;*go.com;*csdn.com;*msn.com;*aliyun.com;*cdn.com;" +
        //"*ttvnw*;*edge*;*microsoft*;*bing*;*google*;*discordapp*;*gstatic.com;*imgur.com;*hub.*;*gitlab.com;*googleapis.com;*facebook.com;*cloudfront.net;*gvt1.com;*jquery.com;*akamai.net;*ultra-rv.com;*youtube*;*ytimg*;*ggpht*;" +
        //"*baidu*;*qq*;*sohu*;*weibo*;*163*;*360*;*iqiyi*;*youku*;*bilibili*;*sogou*;*taobao*;*jd*;*zhihu*;*steam*;*ea.com;*csdn*;*.msn.*;*aliyun*;*cdn*;" +
        //"*twitter.com;*instagram.com;*wikipedia.org;*yahoo*;*xvideos.com;*whatsapp.com;*live.com;*netflix.com;*office.com;*tiktok.com;*reddit.com;*discord*;*twitch*;*duckduckgo.com";

        private static readonly string[] urls =
        {
            "hoyoverse.com",
            "starrails.com",
            "bhsr.com",
            "yuanshen.com",
            "mihoyo.com",
        };

        private static void StartGsProxyServer(int port)
        {
            if (Eavesdropper.IsRunning) return;
            Eavesdropper.Overrides.Clear();
            Eavesdropper.Overrides.AddRange(ProxyOverrides.Split(';'));
            Eavesdropper.RequestInterceptedAsync += EavesdropperOnRequestInterceptedAsync;
            Eavesdropper.Initiate(port);
        }

        private static Task EavesdropperOnRequestInterceptedAsync(object sender, RequestInterceptedEventArgs e)
        {
            var url = e.Request.RequestUri.OriginalString;
            foreach (var mhy in urls)
            {
                var i = url.IndexOf(mhy, StringComparison.CurrentCultureIgnoreCase);
                if (i == -1) continue;
                var p = url.IndexOf('/', i + mhy.Length);
                var target = p >= 0 ? _gcDispatch + url.Substring(p) : _gcDispatch;
                e.Request = RedirectRequest(e.Request as HttpWebRequest, new Uri(target));
                c.Log($"重定向：{url} 到 {e.Request.RequestUri}");
                return Task.CompletedTask;
            }

            c.Log("直连：" + e.Request.RequestUri);

            return Task.CompletedTask;
        }

        private static HttpWebRequest RedirectRequest(HttpWebRequest request, Uri newUri)
        {
            var newRequest = WebRequest.CreateHttp(newUri);
            newRequest.ProtocolVersion = request.ProtocolVersion;
            //newRequest.ProtocolVersion = HttpVersion.Version11;
            newRequest.CookieContainer = request.CookieContainer;
            newRequest.AllowAutoRedirect = request.AllowAutoRedirect;
            newRequest.KeepAlive = request.KeepAlive;
            newRequest.Method = request.Method;
            newRequest.Proxy = request.Proxy;
            foreach (var name in request.Headers.AllKeys)
            {
                switch (name.ToLower())
                {
                    case "host": newRequest.Host = request.Host; break;
                    case "accept": newRequest.Accept = request.Accept; break;
                    case "referer": newRequest.Referer = request.Referer; break;
                    case "user-agent": newRequest.UserAgent = request.UserAgent; break;
                    case "content-type": newRequest.ContentType = request.ContentType; break;
                    case "content-length": newRequest.ContentLength = request.ContentLength; break;
                    case "if-modified-since": newRequest.IfModifiedSince = request.IfModifiedSince; break;
                    case "date": newRequest.Date = request.Date; break;
                    default: newRequest.Headers[name] = request.Headers[name]; break;
                }
            }
            //newRequest.Headers = request.Headers;
            return newRequest;
        }

        private static void StopGsProxyServer()
        {
            Eavesdropper.Terminate();
        }

        #endregion

        private static string _gcDispatch;
        public static void StartProxy(string gcDispatch)
        {
            // 检查Url格式
            var _ = new Uri(gcDispatch);
            _gcDispatch = gcDispatch.TrimEnd('/');
            c.Log("启动代理，重定向到：" + _gcDispatch);
            StartGsProxyServer(主窗口.监听端口);
            SetSystemProxy(主窗口.监听端口);

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public static void StopProxy()
        {
            c.Log("停止代理");
            CloseSystemProxy();
            StopGsProxyServer();
        }

        public static bool CheckAndCreateCertificate()
        {
            return Eavesdropper.Certifier.CreateTrustedRootCertificate();
        }

        public static bool DestroyCertificate()
        {
            return Eavesdropper.Certifier.DestroyTrustedRootCertificate();
        }

        public static bool IsRunning => Eavesdropper.IsRunning;
    }
}
