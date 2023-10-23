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
        public MaterialCharacteristicsDTO Calculate(bool isothermalSinteringStageEnabled)
        {
            _temperature = new Dictionary<double, double>();
            _porosity = new Dictionary<double, double>();
            _density = new Dictionary<double, double>();
            _grainSize = new Dictionary<double, double>();
            _shirnkage = new Dictionary<double, double>();

            double time = 0.0;
            double h = (_tau1 + _tau2) / 1000;
            double T = _t0;
            double k0, k1, k2, k3;

            double L = _l0;
            double P = _p0;
            double roNach = _ro0 * (1 - P);
            double ro = roNach;

            double U = _m * ((1 / roNach) - (1 / ro));

            _grainSize.Add(time / 60, L * 1000000);
            _porosity.Add(time / 60, P * 100);
            _density.Add(time / 60, roNach);
            _shirnkage.Add(time / 60, U * Math.Pow(10, 4));

            for (time = 0; time < _tau1; time = time + h)
            {
                T = CurrentTemperatue(time);
                _temperature.Add(time / 60, T - 273.15);

                k0 = h * dPdt(time, L, P);
                k1 = h * dPdt(time + (h / 2), L, P + (k0 / 2));
                k2 = h * dPdt(time + (h / 2), L, P + (k1 / 2));
                k3 = h * dPdt(time + h, L, P + k2);

                P = P + ((k0 + 2 * k1 + 2 * k2 + k3) / 6);
                _porosity.Add((time + h) / 60, P * 100);

                k0 = h * dLdt(time, L);
                k1 = h * dLdt(time + (h / 2), L + (k0 / 2));
                k2 = h * dLdt(time + (h / 2), L + (k1 / 2));
                k3 = h * dLdt(time + h, L + k2);

                L = L + ((k0 + 2 * k1 + 2 * k2 + k3) / 6);
                _grainSize.Add((time + h) / 60, L * 1000000);

                ro = (1 - P) * _ro0;
                _density.Add((time + h) / 60, ro);

                U = _m * ((1 / roNach) - (1 / ro));
                _shirnkage.Add((time + h) / 60, U * Math.Pow(10, 4));
            }

            if (!isothermalSinteringStageEnabled)
            {
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

            _p20 = P;

            for (; time < _tau1 + _tau2; time = time + h)
            {
                _temperature.Add(time / 60, T - 273.15);

                k0 = h * dP2dt(time, P);
                k1 = h * dP2dt(time + (h / 2), P + (k0 / 2));
                k2 = h * dP2dt(time + (h / 2), P + (k1 / 2));
                k3 = h * dP2dt(time + h, P + k2);

                P = P + ((k0 + 2 * k1 + 2 * k2 + k3) / 6);
                _porosity.Add((time + h) / 60, P * 100);

                k0 = h * dL2dt(time, L);
                k1 = h * dL2dt(time + (h / 2), L + (k0 / 2));
                k2 = h * dL2dt(time + (h / 2), L + (k1 / 2));
                k3 = h * dL2dt(time + h, L + k2);

                L = L + ((k0 + 2 * k1 + 2 * k2 + k3) / 6);
                _grainSize.Add((time + h) / 60, L * 1000000);

                ro = (1 - P) * _ro0;
                _density.Add((time + h) / 60, ro);

                U = _m * ((1 / roNach) - (1 / ro));
                _shirnkage.Add((time + h) / 60, U * Math.Pow(10, 4));
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

        private double CurrentTemperatue(double time)
        {
            var wT = (_tk - _t0) / _tau1;
            return _t0 + wT * time;
        }

        private double dPdt(double t, double L, double P)
        {
            var a = Db(CurrentTemperatue(t)) * _d * _d * _d * _d * _s * P;
            var b = L * L * L * L * _k * CurrentTemperatue(t);
            return -a / b;
        }

        private double Db(double T)
        {
            return _db0 * Math.Exp(-(_eb) / (_r * T));
        }

        private double dLdt(double t, double L)
        {
            var a = 8 * _r * Ds(CurrentTemperatue(t)) * _d * _d * _d * _d * _s;
            var b = L * L * L * _k * _es;
            return (a / b) * (1 + ((_es) / (_r * CurrentTemperatue(t))));
        }

        private double Ds(double T)
        {
            return _ds0 * Math.Exp(-(_es) / (_r * T));
        }

        private double eta(double P)
        {
            return _eta0 * Math.Pow(1 - P, 5.0 / 3);
        }

        private double dP2dt(double t, double P2)
        {
            return -((Pc(P2) + _pg) * (1 - P2)) / hi(P2);
        }

        private double Pc(double P2)
        {
            return 2 * P2 / Rp(P2);
        }

        private double Rp(double P2)
        {
            var a = (1 - _p20) * P2;
            var b = _p20 * (1 - P2);
            return _rp0 * Math.Pow(a / b, 1 / 3);
        }

        private double hi(double P)
        {
            return (400 * eta(P) * (1 - P)) / (3 * P);
        }

        private double dL2dt(double t, double L)
        {
            var a = 8 * Ds(CurrentTemperatue(t)) * _d * _d * _d * _d * _s;
            var b = L * L * L * _k * CurrentTemperatue(t);
            return (a / b);
        }

        #endregion
    }
}