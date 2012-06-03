﻿// Copyright 2011 Chris Patterson, Dru Sellers
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Automatonymous.Tests
{
    using Impl;
    using NUnit.Framework;


    [TestFixture]
    public class When_a_state_is_declared
    {
        [Test]
        public void It_should_capture_the_name_of_final()
        {
            Assert.AreEqual("Final", _machine.Final.Name);
        }

        [Test]
        public void It_should_capture_the_name_of_initial()
        {
            Assert.AreEqual("Initial", _machine.Initial.Name);
        }

        [Test]
        public void It_should_capture_the_name_of_running()
        {
            Assert.AreEqual("Running", _machine.Running.Name);
        }

        [Test]
        public void Should_be_an_instance_of_the_proper_type()
        {
            Assert.IsInstanceOf<StateImpl<StateMachineInstance>>(_machine.Initial);
        }

        TestStateMachine _machine;

        [TestFixtureSetUp]
        public void A_state_is_declared()
        {
            _machine = new TestStateMachine();
        }


        class TestStateMachine :
            AutomatonymousStateMachine<StateMachineInstance>
        {
            public TestStateMachine()
            {
                State(() => Running);
            }

            public State Running { get; private set; }
        }
    }


    [TestFixture]
    public class When_a_state_is_stored_another_way
    {
        [Test]
        public void It_should_get_the_name_right()
        {
            Assert.AreEqual("Running", _instance.CurrentState);
        }


        TestStateMachine _machine;
        Instance _instance;

        [TestFixtureSetUp]
        public void A_state_is_declared()
        {
            _machine = new TestStateMachine();
            _instance = new Instance();
            _instance.StateMachine = _machine;

            _machine.RaiseEvent(_instance, x => x.Started);
        }


        class Instance :
            StateMachineInstance
        {
            StateMachine<Instance> _machine;

            public StateMachine<Instance> StateMachine
            {
                set { _machine = value; }
            }

            public string CurrentState { get; set; }

            State StateMachineInstance.CurrentState
            {
                get
                {
                    if(string.IsNullOrEmpty(CurrentState))
                        return _machine.Initial;

                    return _machine.GetState(CurrentState);
                }
                set { CurrentState = value.Name; }
            }
        }


        class TestStateMachine :
            AutomatonymousStateMachine<Instance>
        {
            public TestStateMachine()
            {
                State(() => Running);
                Event(() => Started);

                Initially(
                    When(Started)
                        .TransitionTo(Running));

            }

            public Event Started { get; private set; }
            public State Running { get; private set; }
        }
    }
}