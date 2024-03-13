using System;
using System.Collections.Generic;

namespace Gladiator_Fights
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BattleField battleField = new BattleField();
            battleField.Fight();
        }
    }

    class BattleField
    {
        private BaseHero _firstFighter;
        private BaseHero _secondFighter;

        private bool _isFighting = true;

        public BattleField()
        {
            _firstFighter = ChooseHero();
            _secondFighter = ChooseHero();
        }

        public void Fight()
        {
            ShowInfo();

            while (_isFighting)
            {
                TryAttack(_firstFighter, _secondFighter);
                TryAttack(_secondFighter, _firstFighter);

                Console.WriteLine();
                ShowInfo();
            }

            DetermineWinner();
        }

        private void TryAttack(BaseHero attacker, BaseHero defencer)
        {
            if (attacker.IsAlive())
                attacker.Attack(defencer);
            else
                _isFighting = false;

        }

        private BaseHero ChooseHero()
        {
            List<BaseHero> Heroes = new List<BaseHero>()
            {
                new Warrior(),
                new Magician(),
                new Spearman(),
                new Knight(),
                new Archer(),
            };

            BaseHero fightingHero = null;

            while (fightingHero == null)
            {
                Console.WriteLine($"1 - выбрать воина, уникальный навык - двойной удар (раз в 3 атаки наносит урон сразу двумя топорами)" +
                $"\n2 - выбрать мага, уникальный навык - огненный шар (когда накопит ману применяет способность)" +
                $"\n3 - выбрать копейщика, уникальный навык - крит (имеет 30% шанс нанести удвоенный урон)" +
                $"\n4 - выбрать рыцаря, уникальный навык - броня (снижает весь входящий урон)" +
                $"\n5 - выбрать лучника, уникальный навык - бей по больному (с каждым выстрелом наносит все больше урона)");

                Console.WriteLine("Введите номер героя");

                string userCommand = Console.ReadLine();

                if (int.TryParse(userCommand, out int numberHero))
                {
                    fightingHero = Heroes[numberHero - 1];
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Введено некорректное значение. Попробуйте снова");
                }
            }

            Console.Clear();

            return fightingHero;
        }

        public void DetermineWinner()
        {
            if (_firstFighter.Health <= 0)
                Console.WriteLine("Победил второй герой");
            else if (_secondFighter.Health <= 0)
                Console.WriteLine("Победил первый герой");
        }

        private void ShowInfo()
        {
            Console.WriteLine(_firstFighter);
            Console.WriteLine(_secondFighter);
        }
    }

    abstract class BaseHero
    {
        protected static Random _random = new Random();
        protected int _damage;
        protected int _maxPercent = 100;

        public int Health { get; protected set; }

        public virtual void Attack(BaseHero enemy)
        {
            enemy.TakeDamage(_damage);
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public bool IsAlive()
        {
            return Health > 0;
        }
    }

    class Warrior : BaseHero
    {
        private int _counterAttack = 0;
        private int _numberAttackRequired = 3;
        private int _spellDamageCoefficient = 2;

        public Warrior()
        {
            Health = 500;
            _damage = 70;
        }

        public override void Attack(BaseHero enemy)
        {
            _counterAttack++;

            if (_counterAttack == _numberAttackRequired)
            {
                _counterAttack = 0;
                enemy.TakeDamage(_damage * _spellDamageCoefficient);
            }
            else
            {
                enemy.TakeDamage(_damage);
            }
        }

        public override string ToString()
        {
            return $"Воин имеет {Health} здоровья, {_damage} урона, атак до применения навыка {_numberAttackRequired - _counterAttack}";
        }

    }

    class Magician : BaseHero
    {
        private int _mana = 0;
        private int _manacost = 100;
        private int _spellDamage = 150;
        private int _manaRegen = 50;

        public Magician()
        {
            Health = 300;
            _damage = 100;
        }

        public override void Attack(BaseHero enemy)
        {
            if (_mana >= _manacost)
            {
                _mana -= _manacost;
                enemy.TakeDamage(_spellDamage);
            }
            else
            {
                _mana += _manaRegen;
                enemy.TakeDamage(_damage);
            }
        }

        public override string ToString()
        {
            return $"Маг имеет {Health} здоровья, {_damage} урона, необходимо маны для применения навыка {_manacost - _mana}";
        }
    }

    class Spearman : BaseHero
    {
        private int _chanceCrit = 30;
        private int _critCoefficient = 2;

        public Spearman()
        {
            Health = 600;
            _damage = 60;
        }

        public override void Attack(BaseHero enemy)
        {
            if (_random.Next(_maxPercent + 1) > _chanceCrit)
                enemy.TakeDamage(_damage);
            else
                enemy.TakeDamage(_damage * _critCoefficient);
        }

        public override string ToString()
        {
            return $"Копейщик имеет {Health} здоровья, {_damage} урона, шанс крита {_chanceCrit}";
        }
    }

    class Knight : BaseHero
    {
        private int _armor = 20;

        public Knight()
        {
            Health = 700;
            _damage = 40;
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage - _armor * _maxPercent / damage;
        }

        public override string ToString()
        {
            return $"Рыцарь имеет {Health} здоровья, {_damage} урона, текущая броня {_armor}";
        }
    }

    class Archer : BaseHero
    {
        private int _numberDamageBeingIncreacing = 25;

        public Archer()
        {
            Health = 400;
            _damage = 50;
        }

        public override void Attack(BaseHero enemy)
        {
            enemy.TakeDamage(_damage);
            _damage += _numberDamageBeingIncreacing;
        }

        public override string ToString()
        {
            return $"Лучник имеет {Health} здоровья, текущий наносимый урон {_damage}, количество увеличиваемого урона {_numberDamageBeingIncreacing}";
        }
    }
}