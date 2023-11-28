using Mathematics.Models;

namespace Mathematics
{
    public class Sintering
    {
        #region Private Properties

        /// <summary>
        /// Начальная температура в печи
        /// </summary>
        private double _t0;
        
        /// <summary>
        /// Конечная температура в печи
        /// </summary>
        private double _tk;
        
        /// <summary>
        /// Начальный средний размер зерна
        /// </summary>
        private double _l0 ;
        
        /// <summary>
        /// Начальный пористость материала
        /// </summary>
        private double _p0 ;
        
        /// <summary>
        /// Время неизотермического спекания
        /// </summary>
        private double _tau1 ;

        /// <summary>
        /// Толщина поверхностного слоя
        /// </summary>
        private double _d;
        
        /// <summary>
        /// Предэкспоненциальный множитель
        /// </summary>
        private double _db0;
        
        /// <summary>
        /// Предэкспоненциальный множитель
        /// </summary>
        private double _ds0;
        
        /// <summary>
        /// Энергия активации
        /// </summary>
        private double _eb;
        
        /// <summary>
        /// Энергия активации
        /// </summary>
        private double _es;
        
        /// <summary>
        /// Плотность компактного материала
        /// </summary>
        private double _ro0;

        /// <summary>
        /// Удельная поверхностная энергия
        /// </summary>
        private double _s;
        
        /// <summary>
        /// Время изотермического спекания
        /// </summary>
        private double _tau2;
        
        /// <summary>
        /// Вязкость беспористого материала
        /// </summary>
        private double _eta0;
                
        /// <summary>
        /// Давление инертного газа
        /// </summary>
        private double _pg;

        /// <summary>
        /// Масса материала
        /// </summary>
        private double _m;

        /// <summary>
        /// Начальная пористость матриала для изотермической стадии
        /// </summary>
        private double _p20;

        #region Константы

        /// <summary>
        /// Постоянная Больцмана
        /// </summary>
        private double _k = 1.38 * Math.Pow(10, -23);

        /// <summary>
        /// Универсальная газовая постоянная
        /// </summary>
        private double _r = 8.31;

        /// <summary>
        /// Начальный радиус поры при спекании под давлением
        /// </summary>
        private double _rp0 = 0.0000009;

        /// <summary>
        /// Конечная пористость, %
        /// </summary>
        private double _pP;

        /// <summary>
        /// Конечный средний размер зерна, мкм
        /// </summary>
        private double _lL;

        /// <summary>
        /// Конечная вязкость материала, МПа*с
        /// </summary>
        private double _ett;

        /// <summary>
        /// Максимальная конечная пористость (Не точно)
        /// </summary>
        private double _pPP;

        /// <summary>
        /// Конечная плотность материала,  кг/м^3
        /// </summary>
        private double _ro;

        #endregion

        #region Точки графиков

        /// <summary>
        /// Температура (x - мин, y - C)
        /// </summary>
        private Dictionary<double, double>? _temperature;

        /// <summary>
        /// Пористость (x - мин, y - кг/м^3)
        /// </summary>
        private Dictionary<double, double>? _porosity;

        /// <summary>
        /// Плотность (x - мин, y - %)
        /// </summary>
        private Dictionary<double, double>? _density;

        /// <summary>
        /// Размер зерна (x - мин, y - мкм)
        /// </summary>
        private Dictionary<double, double>? _grainSize;

        /// <summary>
        /// Усадка (x - мин, y - см^3)
        /// </summary>
        private Dictionary<double, double>? _shirnkage;

        #endregion

        #endregion

        /// <param name="t0">Начальная температура в печи</param>
        /// <param name="tk">Конечная температура в печи</param>
        /// <param name="l0">Начальный средний размер зерна</param>
        /// <param name="p0">Начальный пористость материала</param>
        /// <param name="tau1">Время неизотермического спекания</param>
        /// <param name="d">Толщина поверхностного слоя</param>
        /// <param name="db0">Предэкспоненциальный множитель для коэффициентов зернограничной диффузии</param>
        /// <param name="ds0">Предэкспоненциальный множитель для коэффициентов поверхностной самодиффузии</param>
        /// <param name="eb">Энергии активации роста зерен материала</param>
        /// <param name="es">Энергии активации уплотнения материала</param>
        /// <param name="s">Удельная поверхностная энергия</param>
        /// <param name="eta0">Вязкость беспористого материала</param>
        /// <param name="pg">Давление инертного газа</param>
        /// <param name="m">Масса материала</param>
        /// <param name="ro0">Плотность компактного материала</param>
        /// <param name="tau2">Время изотермического спекания</param>
        public Sintering(double t0, double tk, double l0, double p0, double tau1,
            double d, double db0, double ds0, double eb, double es, double s,
            double eta0, double pg, double m, double ro0, double tau2)
        {
            _t0 = t0;
            _tk = tk;
            _l0 = l0;
            _p0 = p0;
            _tau1 = tau1;
            _d = d;
            _db0 = db0;
            _ds0 = ds0;
            _eb = eb;
            _es = es;
            _s = s;
            _eta0 = eta0;
            _pg = pg;
            _m = m;
            _ro0 = ro0;
            _tau2 = tau2;
        }

        /// <summary>
        /// Расчет характеристик конечно продукта
        /// </summary>
        /// <param name="isothermalSinteringStageEnabled">Включена ли стадия изотермического спекания</param>
        /// <param name="stepsAmount">Количество шагов математической модели</param>
        /// <param name="eps">Погрешность</param>
        public MaterialCharacteristicsDTO Calculate(bool isothermalSinteringStageEnabled, int stepsAmount, double eps)
        {
            _temperature = new Dictionary<double, double>();
            _porosity = new Dictionary<double, double>();
            _density = new Dictionary<double, double>();
            _grainSize = new Dictionary<double, double>();
            _shirnkage = new Dictionary<double, double>();

            var PCharp = 0.0;
            var LCharp = 0.0;
            var PMinusOne = 0.0;
            var LMinusOne = 0.0;

            double time = 0.0;
            double h = (_tau1 + _tau2) / stepsAmount;
            double T = _t0;
            double s1, s2, qMax = 5, trueTime = -h;

            double L = _l0;
            double P = _p0 / 100;
            double roNach = _ro0 * (1 - P);
            double ro = roNach;

            double U = _m * ((1 / roNach) - (1 / ro));

            _grainSize.Add(time / 60, L * 1000000);
            _porosity.Add(time / 60, P * 100);
            _density.Add(time / 60, roNach);
            _shirnkage.Add(time / 60, U * Math.Pow(10, 4));

            for (time = trueTime + h; time < _tau1; time = trueTime + h)
            {
                var trueP = P;
                var trueL = L;
                trueTime = time;
                s1 = 0;
                s2 = 0;

                while (true)
                {
                    P = trueP;
                    L = trueL;
                    time = trueTime;

                    while (true)
                    {
                        time += h;
                        T = dT1dt(time);
                        PMinusOne = P;
                        LMinusOne = L;
                        P = PorosityFirstStage(time, L, P, h);
                        L = AvgGrainSizeFirstStage(time, L, h);

                        if (s1 == 0)
                        {
                            PCharp = P;
                            LCharp = L;
                            h = h / 2;
                            s1 = 1;
                            continue;
                        }

                        if (s2 == 0)
                        {
                            time -= h;
                            P = PMinusOne;
                            L = LMinusOne;
                            s2 = 1;
                            continue;
                        }

                        break;
                    }

                    var epsJ = Math.Max((Math.Abs(PCharp - P) / P * 100), (Math.Abs(LCharp - L) / L * 100));
                    if(epsJ > eps)
                    {
                        if (s1 >= qMax)
                        {
                            return new MaterialCharacteristicsDTO()
                            {
                                Ett = 0,
                                LL = 0,
                                PP = 0,
                                PPP = 0,
                                Ro = 0
                            };
                        }

                        PCharp = P;
                        LCharp = L;
                        h = h / 2;
                        s2 = 0;
                        s1++;
                        continue;
                    }

                    ro = (1 - P) * _ro0;
                    U = _m * ((1 / roNach) - (1 / ro));

                    if (epsJ < eps / 4)
                    {
                        h = h * 2;
                    }

                    break;
                }

                _temperature.Add((trueTime + h) / 60, T);                
                _porosity.Add((trueTime + h) / 60, P * 100);                
                _grainSize.Add((trueTime + h) / 60, L * 1000000);                
                _density.Add((trueTime + h) / 60, ro);
                _shirnkage.Add((trueTime + h) / 60, U * Math.Pow(10, 4));
            }

            if (!isothermalSinteringStageEnabled)
            {
                _pP = P * 100;
                _lL = L * 1000000;
                _ett = eta(P) / 1000000;
                _pPP = Math.Floor(_pP);
                _ro = ro;

                return new MaterialCharacteristicsDTO()
                {
                    Ett = _ett,
                    LL = _lL,
                    PP = _pP,
                    PPP = _pPP,
                    Ro = _ro,
                };
            }

            _p20 = P;


            if (!isothermalSinteringStageEnabled)
            {
                _pP = P * 100;
                _lL = L * 1000000;
                _ett = eta(P) / 1000000;
                _pPP = Math.Floor(_pP);
                _ro = ro;

                return new MaterialCharacteristicsDTO()
                {
                    Ett = _ett,
                    LL = _lL,
                    PP = _pP,
                    PPP = _pPP,
                    Ro = _ro,
                };
            }

            for (; time < _tau1 + _tau2; time = trueTime + h)
            {
                var trueP = P;
                var trueL = L;
                trueTime = time;
                s1 = 0;
                s2 = 0;

                while (true)
                {
                    P = trueP;
                    L = trueL;
                    time = trueTime;

                    while (true)
                    {
                        time += h;
                        PMinusOne = P;
                        LMinusOne = L;
                        P = PorositySecondStage(P, h);
                        L = AvgGrainSizeSecondStage(L, h);

                        if (s1 == 0)
                        {
                            PCharp = P;
                            LCharp = L;
                            h /= 2;
                            s1 = 1;
                            continue;
                        }

                        if (s2 == 0)
                        {
                            time -= h;
                            P = PMinusOne;
                            L = LMinusOne;
                            s2 = 1;
                            continue;
                        }

                        break;
                    }

                    var epsJ = Math.Max((Math.Abs(PCharp - P) / P * 100), (Math.Abs(LCharp - L) / L * 100));
                    if (epsJ > eps)
                    {
                        if (s1 >= qMax)
                        {
                            return new MaterialCharacteristicsDTO()
                            {
                                Ett = 0,
                                LL = 0,
                                PP = 0,
                                PPP = 0,
                                Ro = 0
                            };
                        }

                        PCharp = P;
                        LCharp = L;
                        h = h / 2;
                        s2 = 0;
                        s1++;
                        continue;
                    }

                    ro = (1 - P) * _ro0;
                    U = _m * ((1 / roNach) - (1 / ro));

                    if (epsJ < eps / 4)
                    {
                        h *= 2;
                    }

                    break;
                }

                _temperature.Add((trueTime + h) / 60, T);
                _porosity.Add((trueTime + h) / 60, P * 100);
                _grainSize.Add((trueTime + h) / 60, L * 1000000);
                _density.Add((trueTime + h) / 60, ro);
                _shirnkage.Add((trueTime + h) / 60, U * Math.Pow(10, 4));
            }

            _pP = P * 100;
            _lL = L / 0.000001;
            _ett = eta(P) / 1000000;
            _pPP = Math.Floor(_pP);
            _ro = ro;

            return new MaterialCharacteristicsDTO()
            {
                Ett = _ett,
                LL = _lL,
                PP = _pP,
                PPP = _pPP,
                Ro = _ro,
            };
        }

        /// <summary>
        /// Значения для графика температуры
        /// </summary>
        /// <returns></returns>
        public Dictionary<double, double> GetTemperatureChartValues()
            => _temperature ?? new Dictionary<double, double>();

        /// <summary>
        /// Значенияи для графика пористости
        /// </summary>
        /// <returns></returns>
        public Dictionary<double, double> GetPorosityChartValues()
            => _porosity ?? new Dictionary<double, double>();

        /// <summary>
        /// Значения для графика плотности
        /// </summary>
        /// <returns></returns>
        public Dictionary<double, double> GetDensityChartValues()
            => _density ?? new Dictionary<double, double>();

        /// <summary>
        /// Значения для графика усадки
        /// </summary>
        /// <returns></returns>
        public Dictionary<double, double> GetShinkageChartValues()
            => _shirnkage ?? new Dictionary<double, double>();

        /// <summary>
        /// Значения для графика размера зерна
        /// </summary>
        /// <returns></returns>
        public Dictionary<double, double> GetGrainSizeChartValues()
            => _grainSize ?? new Dictionary<double, double>();


        #region Private Methods

        private double dT1dt(double time)
        {
            var wT = (_tk - _t0) / (_tau1);
            return _t0 + wT * time;
            //return wT;
        }

        private double dT2dt(double time)
        {
            return _tk;
        }

        private double dPdt(double t, double L, double P)
        {
            var a = Db(dT1dt(t)) * _d * _d * _d * _d * _s * P;
            var b = L * L * L * L * _k * (dT1dt(t) + 273);
            return -a / b;
        }

        private double Db(double T)
        {
            return _db0 * Math.Exp(-_eb / (_r * (T + 273)));
        }

        private double dLdt(double t, double L)
        {
            var a = 8 * _r * Ds(dT1dt(t)) * _d * _d * _d * _d * _s;
            var b = L * L * L * _k * _es;
            return (a / b) * (1 + (_es / (_r * (dT1dt(t) + 273))));
        }

        private double Ds(double T)
        {
            return _ds0 * Math.Exp(-(_es) / (_r * (T + 273)));
        }

        private double eta(double P)
        {
            var temp = _eta0 * Math.Pow(1 - P, 5.0 / 3);
            return _eta0 * Math.Pow(1 - P, 5.0 / 3);
        }

        private double dP2dt(double t, double P2)
        {
            var temp = hi(P2);
            return -((Pc(P2) + _pg) * (1 - P2)) / hi(P2);
        }

        private double Pc(double P2)
        {
            var temp = 2 * P2 * _s / Rp(P2);
            return 2 * P2 * _s / Rp(P2);
        }

        private double Rp(double P2)
        {
            var a = (1 - _p20) * P2;
            var b = _p20 * (1 - P2);
            var temp = _rp0 * Math.Pow(a / b, 1.0 / 3);
            return _rp0 * Math.Pow(a / b, 1.0 / 3);
        }

        private double hi(double P)
        {
            var temp = (400 * eta(P) * (1 - P)) / (3 * P);
            return (400 * eta(P) * (1 - P)) / (3 * P);
        }

        private double dL2dt(double t, double L)
        {
            var a = 8 * Ds(dT2dt(t) + 273) * _d * _d * _d * _d * _s;
            var b = L * L * L * _k * (dT2dt(t) + 273);
            return (a / b);
        }

        private double PorosityFirstStage(double time, double L, double P, double h)
        {
            var k0 = dPdt(time, L, P);
            var k1 = dPdt(time + (h / 2), L, P + (h * k0 / 2));
            var k2 = dPdt(time + (h / 2), L, P + (h * k1 / 2));
            var k3 = dPdt(time + h, L, P + h * k2);

            return P + (h * (k0 + 2 * k1 + 2 * k2 + k3) / 6);
        }

        private double PorositySecondStage(double P, double h)
        {
            var k0 = dP2dt(0, P);
            var k1 = dP2dt(0, P + (h * k0 / 2));
            var k2 = dP2dt(0, P + (h * k1 / 2));
            var k3 = dP2dt(0, P + k2 * h);

            return P + (h * (k0 + 2 * k1 + 2 * k2 + k3) / 6);
        }

        private double AvgGrainSizeFirstStage(double time, double L, double h)
        {
            var k0 = dLdt(time, L);
            var k1 = dLdt(time + (h / 2), L + (k0 * h / 2));
            var k2 = dLdt(time + (h / 2), L + (k1 * h / 2));
            var k3 = dLdt(time + h, L + h * k2);

            return L + (h * (k0 + 2 * k1 + 2 * k2 + k3) / 6);
        }

        private double AvgGrainSizeSecondStage(double L, double h)
        {
            var k0 = dL2dt(0, L);
            var k1 = dL2dt(0, L + (h / 2));
            var k2 = dL2dt(0, L + (h / 2));
            var k3 = dL2dt(0, L + h);

            return L + (h * (k0 + 2 * k1 + 2 * k2 + k3) / 6);
        }

        #endregion
    }
}